using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Drivers.Stubs;

/// <summary>Заглушка вентилятора (OnOff + скорость fan_speed + осцилляция).</summary>
public sealed class StubFan(string id, StubDeviceConfig config) : IDevice
{
    private bool isOn;
    private FanSpeed speed = FanSpeed.Auto;
    private bool oscillating;

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
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-fan" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.Fan,
            Capabilities =
            [
                new OnOffCapability { Instance = OnOffCapability.InstanceName },
                new FanSpeedCapability { Instance = FanSpeedCapability.InstanceName, Speeds = [FanSpeed.Auto, FanSpeed.Low, FanSpeed.Medium, FanSpeed.High] },
                new OscillationCapability { Instance = OscillationCapability.InstanceName },
            ],
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
                new FanSpeedState { Instance = FanSpeedCapability.InstanceName, Value = speed },
                new OscillationState { Instance = OscillationCapability.InstanceName, Value = oscillating },
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
