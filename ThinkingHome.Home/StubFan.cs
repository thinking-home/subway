using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>Заглушка вентилятора (OnOff + скорость fan_speed + осцилляция).</summary>
public sealed class StubFan(string id, string title, string? room = null) : IDevice
{
    private bool isOn;
    private FanSpeed speed = FanSpeed.Auto;
    private bool oscillating;

    public string Id => id;

    public event Action<StateChange>? Changed;

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-fan" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Types = [DeviceType.Fan],
            Capabilities =
            [
                new OnOffCapability { Instance = "on_off" },
                new FanSpeedCapability { Instance = "fan_speed", Speeds = [FanSpeed.Auto, FanSpeed.Low, FanSpeed.Medium, FanSpeed.High] },
                new OscillationCapability { Instance = "oscillation" },
            ],
        }],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new OnOffState { Instance = "on_off", Value = isOn },
                new FanSpeedState { Instance = "fan_speed", Value = speed },
                new OscillationState { Instance = "oscillation", Value = oscillating },
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
