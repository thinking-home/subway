using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Drivers.Stubs;

/// <summary>Заглушка кондиционера (OnOff + уставка температуры + режим + скорость + осцилляция + сенсор комнатной температуры).</summary>
public sealed class StubAirConditioner(string id, StubDeviceConfig config) : IDevice
{
    private bool isOn;
    private int targetCelsius = 23;
    private ThermostatMode mode = ThermostatMode.Cool;
    private FanSpeed speed = FanSpeed.Auto;
    private bool oscillating;
    private readonly double roomCelsius = 26.5;

    /// <inheritdoc />
    public string Id => id;

    /// <inheritdoc />
    public event Action<StateChange>? Changed;

    /// <inheritdoc />
    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = config.Title,
        Room = config.Room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-ac" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.AirConditioner,
            Capabilities =
            [
                new OnOffCapability { Instance = OnOffCapability.InstanceName },
                new TargetTemperatureCapability { Instance = TargetTemperatureCapability.InstanceName, MinCelsius = 18, MaxCelsius = 33 },
                new ThermostatModeCapability { Instance = ThermostatModeCapability.InstanceName, Modes = [ThermostatMode.Auto, ThermostatMode.Heat, ThermostatMode.Cool, ThermostatMode.Dry, ThermostatMode.FanOnly] },
                new FanSpeedCapability { Instance = FanSpeedCapability.InstanceName, Speeds = [FanSpeed.Auto, FanSpeed.Low, FanSpeed.Medium, FanSpeed.High] },
                new OscillationCapability { Instance = OscillationCapability.InstanceName },
            ],
            Properties = [new TemperatureProperty { Instance = TemperatureProperty.InstanceName }],
        }],
    };

    /// <inheritdoc />
    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new OnOffState { Instance = OnOffCapability.InstanceName, Value = isOn },
                new TargetTemperatureState { Instance = TargetTemperatureCapability.InstanceName, Value = targetCelsius },
                new ThermostatModeState { Instance = ThermostatModeCapability.InstanceName, Value = mode },
                new FanSpeedState { Instance = FanSpeedCapability.InstanceName, Value = speed },
                new OscillationState { Instance = OscillationCapability.InstanceName, Value = oscillating },
                new TemperatureState { Instance = TemperatureProperty.InstanceName, Value = roomCelsius },
            ],
        });

    /// <inheritdoc />
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
    {
        switch (command)
        {
            case OnOffCommand on:
                isOn = on.Value;
                Console.WriteLine($"[{id}] → {(isOn ? "ВКЛ" : "выкл")}");
                Report(new OnOffState { Instance = OnOffCapability.InstanceName, Value = isOn });
                return Task.FromResult(CommandOutcome.Done);

            case TargetTemperatureCommand temp:
                targetCelsius = temp.Value;
                Console.WriteLine($"[{id}] → уставка {targetCelsius} °C");
                Report(new TargetTemperatureState { Instance = TargetTemperatureCapability.InstanceName, Value = targetCelsius });
                return Task.FromResult(CommandOutcome.Done);

            case ThermostatModeCommand m:
                mode = m.Value;
                Console.WriteLine($"[{id}] → режим {mode}");
                Report(new ThermostatModeState { Instance = ThermostatModeCapability.InstanceName, Value = mode });
                return Task.FromResult(CommandOutcome.Done);

            case FanSpeedCommand fan:
                speed = fan.Value;
                Console.WriteLine($"[{id}] → скорость {speed}");
                Report(new FanSpeedState { Instance = FanSpeedCapability.InstanceName, Value = speed });
                return Task.FromResult(CommandOutcome.Done);

            case OscillationCommand osc:
                oscillating = osc.Value;
                Console.WriteLine($"[{id}] → осцилляция {(oscillating ? "ВКЛ" : "выкл")}");
                Report(new OscillationState { Instance = OscillationCapability.InstanceName, Value = oscillating });
                return Task.FromResult(CommandOutcome.Done);

            default:
                return Task.FromResult(CommandOutcome.Unsupported);
        }
    }

    private void Report(StateValue value) => Changed?.Invoke(new StateChange { DeviceId = id, Value = value });
}
