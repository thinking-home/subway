using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.FluentApi;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

public class FluentApiTests
{
    // --- команды: строго 1:1 с командами ядра ---

    [Fact]
    public async Task Every_capability_method_builds_its_core_command()
    {
        var host = new FakeHost();
        var device = host.Device("lamp");

        await device.OnOff().TurnOnAsync();
        await device.OnOff().TurnOffAsync();
        await device.OnOff().SetAsync(true);
        await device.Brightness().SetAsync(50);
        await device.Color().SetRgbAsync(0xFF0000);
        await device.Color().SetTemperatureAsync(4000);
        await device.Open().SetAsync(75);
        await device.FanSpeed().SetAsync(FanSpeed.High);
        await device.Oscillation().SetAsync(true);
        await device.ThermostatMode().SetAsync(ThermostatMode.Cool);
        await device.TargetTemperature().SetAsync(23);

        // канонические instance проверяем строками: это контракт, а не деталь реализации
        Assert.Equal(11, host.Executed.Count);
        Assert.True(host.Executed[0] is OnOffCommand { Instance: "on_off", Value: true });
        Assert.True(host.Executed[1] is OnOffCommand { Instance: "on_off", Value: false });
        Assert.True(host.Executed[2] is OnOffCommand { Instance: "on_off", Value: true });
        Assert.True(host.Executed[3] is BrightnessCommand { Instance: "brightness", Value: 50 });
        Assert.True(host.Executed[4] is ColorRgbCommand { Instance: "color", Value: 0xFF0000 });
        Assert.True(host.Executed[5] is ColorTemperatureCommand { Instance: "color", Value: 4000 });
        Assert.True(host.Executed[6] is OpenCommand { Instance: "open", Value: 75 });
        Assert.True(host.Executed[7] is FanSpeedCommand { Instance: "fan_speed", Value: FanSpeed.High });
        Assert.True(host.Executed[8] is OscillationCommand { Instance: "oscillation", Value: true });
        Assert.True(host.Executed[9] is ThermostatModeCommand { Instance: "thermostat_mode", Value: ThermostatMode.Cool });
        Assert.True(host.Executed[10] is TargetTemperatureCommand { Instance: "target_temperature", Value: 23 });
        Assert.All(host.Executed, c => Assert.Equal(0, c.EndpointId)); // шорткаты бьют в endpoint 0
    }

    [Fact]
    public async Task Endpoint_shortcut_is_strict_synonym_of_endpoint0()
    {
        var host = new FakeHost();
        var device = host.Device("dev");

        await device.OnOff().TurnOnAsync();               // шорткат
        await device.Endpoint(0).OnOff().TurnOnAsync();   // явный 0
        await device.Endpoint(1).OnOff().TurnOnAsync();   // другой endpoint

        Assert.Equal([0, 0, 1], host.Executed.Select(c => c.EndpointId));
    }

    [Fact]
    public async Task Command_outcome_is_passed_through_unchanged()
    {
        var host = new FakeHost { Outcome = CommandOutcome.Unsupported };

        var outcome = await host.Device("lamp").Brightness().SetAsync(50);

        Assert.Same(CommandOutcome.Unsupported, outcome);
    }

    // --- чтение: Query + выбор своего значения из снапшота ---

    [Fact]
    public async Task GetAsync_picks_value_by_endpoint_instance_and_type()
    {
        // двухканальный счётчик: одинаковые свойства на разных endpoint'ах + батарея на нулевом
        var host = new FakeHost
        {
            Snapshot = new DeviceSnapshot
            {
                DeviceId = "waterius",
                Values =
                [
                    new WaterMeterState { EndpointId = 0, Instance = "water_meter", Value = 123.456 },
                    new BatteryState { EndpointId = 0, Instance = "battery", Value = 74 },
                    new WaterMeterState { EndpointId = 1, Instance = "water_meter", Value = 45.678 },
                ],
            },
        };
        var device = host.Device("waterius");

        Assert.Equal(123.456, await device.WaterMeter().GetAsync());               // шорткат ≡ endpoint 0
        Assert.Equal(45.678, await device.Endpoint(1).WaterMeter().GetAsync());
        Assert.Equal(74, await device.Battery().GetAsync());
    }

    [Fact]
    public async Task GetAsync_returns_null_when_value_is_absent()
    {
        var host = new FakeHost
        {
            Snapshot = new DeviceSnapshot
            {
                DeviceId = "lamp",
                Values = [new OnOffState { Instance = "on_off", Value = true }],
            },
        };
        var device = host.Device("lamp");

        Assert.Null(await device.Brightness().GetAsync());          // способности нет
        Assert.Null(await device.Temperature().GetAsync());         // свойства нет
        Assert.Null(await device.Endpoint(1).OnOff().GetAsync());   // не тот endpoint
        Assert.True(await device.OnOff().GetAsync());               // а своё значение находится
    }

    [Fact]
    public async Task Color_representations_share_one_slot()
    {
        var host = new FakeHost
        {
            Snapshot = new DeviceSnapshot
            {
                DeviceId = "lamp",
                Values = [new ColorTemperatureState { Instance = ColorCapability.InstanceName, Value = 4000 }],
            },
        };
        var color = host.Device("lamp").Color();

        Assert.Equal(4000, await color.GetTemperatureAsync());
        Assert.Null(await color.GetRgbAsync()); // активно другое представление
    }

    [Fact]
    public async Task GetAsync_throws_on_unknown_device() // «нет устройства» — исключение, как в ядре
    {
        var host = new DeviceHost();

        await Assert.ThrowsAsync<KeyNotFoundException>(() => host.Device("nope").Temperature().GetAsync());
    }

    // --- discovery: DescribeAsync возвращает типизированный фрагмент дескриптора или null ---

    [Fact]
    public async Task DescribeAsync_returns_typed_fragment_or_null()
    {
        var host = new FakeHost
        {
            Descriptor = new DeviceDescriptor
            {
                Id = "lamp",
                Title = "Лампа",
                Endpoints =
                [
                    new Endpoint
                    {
                        Id = 0,
                        Type = DeviceType.ExtendedColorLight,
                        Capabilities =
                        [
                            new OnOffCapability { Instance = "on_off" },
                            new ColorCapability
                            {
                                Instance = ColorCapability.InstanceName,
                                Temperature = new ColorTemperatureRange { MinKelvin = 2700, MaxKelvin = 6500 },
                            },
                        ],
                        Properties = [new BatteryProperty { Instance = "battery" }],
                    },
                ],
            },
        };
        var device = host.Device("lamp");

        Assert.NotNull(await device.DescribeAsync());
        Assert.Null(await host.Device("nope").DescribeAsync());

        Assert.NotNull(await device.Endpoint(0).DescribeAsync());
        Assert.Null(await device.Endpoint(5).DescribeAsync());

        var color = await device.Color().DescribeAsync();
        Assert.Equal(2700, color?.Temperature?.MinKelvin);

        Assert.NotNull(await device.OnOff().DescribeAsync());
        Assert.NotNull(await device.Battery().DescribeAsync());
        Assert.Null(await device.Brightness().DescribeAsync());            // способности нет
        Assert.Null(await host.Device("nope").OnOff().DescribeAsync());    // устройства нет → тоже null
    }

    // --- подписки: уровни «хост» и «устройство», отписка — Dispose ---

    [Fact]
    public void Device_subscription_filters_by_id_and_dispose_unsubscribes()
    {
        var host = new FakeHost();
        var received = new List<StateChange>();

        var subscription = host.Device("lamp").OnChanged(received.Add);
        host.Raise(Change("lamp", true));
        host.Raise(Change("other", true)); // чужое — не должно прийти

        var change = Assert.Single(received);
        Assert.Equal("lamp", change.DeviceId);

        subscription.Dispose();
        host.Raise(Change("lamp", false));
        Assert.Single(received); // после Dispose ничего не приходит
    }

    [Fact]
    public void Host_subscription_receives_changes_of_all_devices()
    {
        var host = new FakeHost();
        var received = new List<StateChange>();

        using (host.OnChanged(received.Add))
        {
            host.Raise(Change("lamp", true));
            host.Raise(Change("other", false));
        }

        host.Raise(Change("lamp", false)); // уже после Dispose

        Assert.Equal(["lamp", "other"], received.Select(c => c.DeviceId));
    }

    // --- сквозной сценарий поверх настоящего DeviceHost ---

    [Fact]
    public async Task FluentApi_drives_real_host_end_to_end()
    {
        var host = new DeviceHost();
        host.Register(new StubLamp("lamp"));
        var lamp = host.Device("lamp");

        var received = new List<StateChange>();
        using var subscription = lamp.OnChanged(received.Add);

        Assert.NotNull(await lamp.OnOff().DescribeAsync());
        Assert.Null(await lamp.Brightness().DescribeAsync());

        var outcome = await lamp.OnOff().TurnOnAsync();
        Assert.Equal(CommandStatus.Done, outcome.Status);
        Assert.True(await lamp.OnOff().GetAsync());
        Assert.True(Assert.IsType<OnOffState>(Assert.Single(received).Value).Value);

        // способности нет → Unsupported как данные, не исключение
        var unsupported = await lamp.Brightness().SetAsync(50);
        Assert.Equal(CommandErrorCode.NotSupported, unsupported.ErrorCode);
    }

    private static StateChange Change(string deviceId, bool value)
        => new() { DeviceId = deviceId, Value = new OnOffState { Instance = "on_off", Value = value } };

    /// <summary>Записывающий хост: fluent API должен сводиться ровно к вызовам пяти членов ядра.</summary>
    private sealed class FakeHost : IDeviceHost
    {
        public List<DeviceCommand> Executed { get; } = [];
        public DeviceDescriptor? Descriptor { get; set; }
        public DeviceSnapshot? Snapshot { get; set; }
        public CommandOutcome Outcome { get; set; } = CommandOutcome.Done;

        public event Action<StateChange>? Changed;

        public Task<IReadOnlyCollection<DeviceDescriptor>> GetDevicesAsync(CancellationToken ct = default)
            => Task.FromResult<IReadOnlyCollection<DeviceDescriptor>>(Descriptor is null ? [] : [Descriptor]);

        public Task<DeviceDescriptor?> GetDeviceAsync(string deviceId, CancellationToken ct = default)
            => Task.FromResult(Descriptor is { } d && d.Id == deviceId ? d : null);

        public Task<DeviceSnapshot> QueryAsync(string deviceId, CancellationToken ct = default)
            => Snapshot is { } s && s.DeviceId == deviceId
                ? Task.FromResult(s)
                : throw new KeyNotFoundException($"Device '{deviceId}' is not registered.");

        public Task<CommandOutcome> ExecuteAsync(string deviceId, DeviceCommand command, CancellationToken ct = default)
        {
            Executed.Add(command);
            return Task.FromResult(Outcome);
        }

        public void Raise(StateChange change) => Changed?.Invoke(change);
    }

    private sealed class StubLamp(string id) : IDevice
    {
        private bool isOn;

        public string Id => id;
        public event Action<StateChange>? Changed;

        public DeviceDescriptor Describe() => new()
        {
            Id = id,
            Title = id,
            Endpoints = [new Endpoint
            {
                Id = 0,
                Type = DeviceType.OnOffLight,
                Capabilities = [new OnOffCapability { Instance = OnOffCapability.InstanceName }],
            }],
        };

        public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
            => Task.FromResult(new DeviceSnapshot
            {
                DeviceId = id,
                Values = [new OnOffState { Instance = OnOffCapability.InstanceName, Value = isOn }],
            });

        public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        {
            if (command is not OnOffCommand cmd) return Task.FromResult(CommandOutcome.Unsupported);

            isOn = cmd.Value;
            Changed?.Invoke(new StateChange
            {
                DeviceId = id,
                Value = new OnOffState { Instance = OnOffCapability.InstanceName, Value = isOn },
            });
            return Task.FromResult(CommandOutcome.Done);
        }
    }
}
