using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>Заглушка кондиционера (OnOff + уставка температуры + режим + скорость + осцилляция + сенсор комнатной температуры).</summary>
public sealed class StubAirConditioner(string id, string title, string? room = null) : IDevice
{
    private bool isOn;
    private int targetCelsius = 23;
    private ThermostatMode mode = ThermostatMode.Cool;
    private FanSpeed speed = FanSpeed.Auto;
    private bool oscillating;
    private readonly double roomCelsius = 26.5;

    public string Id => id;

    public event Action<StateChange>? Changed;

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-ac" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Types = [DeviceType.AirConditioner],
            Capabilities =
            [
                new OnOffCapability { Instance = "on_off" },
                new TargetTemperatureCapability { Instance = "target_temperature", MinCelsius = 18, MaxCelsius = 33 },
                new ThermostatModeCapability { Instance = "thermostat_mode", Modes = [ThermostatMode.Auto, ThermostatMode.Heat, ThermostatMode.Cool, ThermostatMode.Dry, ThermostatMode.FanOnly] },
                new FanSpeedCapability { Instance = "fan_speed", Speeds = [FanSpeed.Auto, FanSpeed.Low, FanSpeed.Medium, FanSpeed.High] },
                new OscillationCapability { Instance = "oscillation" },
            ],
            Properties = [new TemperatureProperty { Instance = "temperature" }],
        }],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new OnOffState { Instance = "on_off", Value = isOn },
                new TargetTemperatureState { Instance = "target_temperature", Value = targetCelsius },
                new ThermostatModeState { Instance = "thermostat_mode", Value = mode },
                new FanSpeedState { Instance = "fan_speed", Value = speed },
                new OscillationState { Instance = "oscillation", Value = oscillating },
                new TemperatureState { Instance = "temperature", Value = roomCelsius },
            ],
        });

    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
    {
        switch (command)
        {
            case OnOffCommand on:
                isOn = on.Value;
                Console.WriteLine($"[{id}] → {(isOn ? "ВКЛ" : "выкл")}");
                Report(new OnOffState { Instance = "on_off", Value = isOn });
                return Task.FromResult(CommandOutcome.Done);

            case TargetTemperatureCommand temp:
                targetCelsius = temp.Value;
                Console.WriteLine($"[{id}] → уставка {targetCelsius} °C");
                Report(new TargetTemperatureState { Instance = "target_temperature", Value = targetCelsius });
                return Task.FromResult(CommandOutcome.Done);

            case ThermostatModeCommand m:
                mode = m.Value;
                Console.WriteLine($"[{id}] → режим {mode}");
                Report(new ThermostatModeState { Instance = "thermostat_mode", Value = mode });
                return Task.FromResult(CommandOutcome.Done);

            case FanSpeedCommand fan:
                speed = fan.Value;
                Console.WriteLine($"[{id}] → скорость {speed}");
                Report(new FanSpeedState { Instance = "fan_speed", Value = speed });
                return Task.FromResult(CommandOutcome.Done);

            case OscillationCommand osc:
                oscillating = osc.Value;
                Console.WriteLine($"[{id}] → осцилляция {(oscillating ? "ВКЛ" : "выкл")}");
                Report(new OscillationState { Instance = "oscillation", Value = oscillating });
                return Task.FromResult(CommandOutcome.Done);

            default:
                return Task.FromResult(CommandOutcome.Unsupported);
        }
    }

    private void Report(StateValue value) => Changed?.Invoke(new StateChange { DeviceId = id, Value = value });
}
