using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

public class DeviceHostTests
{
    [Fact]
    public async Task Concurrent_first_queries_hit_the_driver_once() // single-flight
    {
        var host = new DeviceHost();
        var device = new StubDevice("lamp");
        host.Register(device);

        var results = await Task.WhenAll(Enumerable.Range(0, 8).Select(_ => host.QueryAsync("lamp")));

        Assert.Equal(8, results.Length);
        Assert.Equal(1, device.QueryCount);
    }

    [Fact]
    public async Task Report_updates_cache_without_hitting_driver_again()
    {
        var host = new DeviceHost();
        var device = new StubDevice("lamp");
        host.Register(device);

        _ = await host.QueryAsync("lamp"); // прайминг (1 обращение к драйверу)
        await host.ExecuteAsync("lamp", new OnOffCommand { Instance = "on_off", Value = true }); // → Report обновляет кэш

        var snapshot = await host.QueryAsync("lamp"); // из кэша
        var state = Assert.IsType<OnOffState>(Assert.Single(snapshot.Values));
        Assert.True(state.Value);
        Assert.Equal(1, device.QueryCount);
    }

    [Fact]
    public async Task Unknown_device_throws()
    {
        var host = new DeviceHost();
        await Assert.ThrowsAsync<KeyNotFoundException>(() => host.QueryAsync("nope"));
    }

    [Fact]
    public async Task Color_switch_overwrites_previous_representation() // взаимоисключающие представления цвета делят один слот кэша
    {
        var host = new DeviceHost();
        var device = new ColorModeDevice("lamp");
        host.Register(device);

        _ = await host.QueryAsync("lamp"); // прайминг: температура (instance "color")
        await host.ExecuteAsync("lamp", new ColorRgbCommand { Instance = ColorCapability.InstanceName, Value = 0xFF0000 });

        var snapshot = await host.QueryAsync("lamp");
        var state = Assert.IsType<ColorRgbState>(Assert.Single(snapshot.Values)); // ровно одно, и это rgb
        Assert.Equal(0xFF0000, state.Value);
    }

    private sealed class StubDevice(string id) : IDevice
    {
        private bool isOn;
        public int QueryCount;

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
                Capabilities = [new OnOffCapability { Instance = "on_off" }],
            }],
        };

        public async Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        {
            Interlocked.Increment(ref QueryCount);
            await Task.Delay(30, ct); // окно для проверки single-flight
            return new DeviceSnapshot { DeviceId = id, Values = [new OnOffState { Instance = "on_off", Value = isOn }] };
        }

        public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        {
            if (command is OnOffCommand cmd)
            {
                isOn = cmd.Value;
                Changed?.Invoke(new StateChange
                {
                    DeviceId = id,
                    Value = new OnOffState { Instance = "on_off", Value = isOn },
                });
                return Task.FromResult(CommandOutcome.Done);
            }

            return Task.FromResult(CommandOutcome.Unsupported);
        }
    }

    // цветное устройство: прайминг в режиме температуры, затем переключение в rgb (тот же instance "color")
    private sealed class ColorModeDevice(string id) : IDevice
    {
        private StateValue color = new ColorTemperatureState { Instance = ColorCapability.InstanceName, Value = 4000 };

        public string Id => id;
        public event Action<StateChange>? Changed;

        public DeviceDescriptor Describe() => new()
        {
            Id = id,
            Title = id,
            Endpoints = [new Endpoint
            {
                Id = 0,
                Type = DeviceType.ExtendedColorLight,
                Capabilities = [new ColorCapability
                {
                    Instance = ColorCapability.InstanceName,
                    Model = ColorModel.Rgb,
                    Temperature = new ColorTemperatureRange { MinKelvin = 2700, MaxKelvin = 6500 },
                }],
            }],
        };

        public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
            => Task.FromResult(new DeviceSnapshot { DeviceId = id, Values = [color] });

        public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        {
            if (command is ColorRgbCommand rgb)
            {
                color = new ColorRgbState { Instance = ColorCapability.InstanceName, Value = rgb.Value };
                Changed?.Invoke(new StateChange { DeviceId = id, Value = color });
                return Task.FromResult(CommandOutcome.Done);
            }

            return Task.FromResult(CommandOutcome.Unsupported);
        }
    }
}
