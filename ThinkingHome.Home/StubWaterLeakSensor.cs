using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>Заглушка датчика протечки (только чтение). true — протечка обнаружена.</summary>
public sealed class StubWaterLeakSensor(string id, string title, string? room = null) : IDevice
{
    private readonly bool leaking = false;
    private readonly double battery = 92;

    public string Id => id;

    public event Action<StateChange>? Changed;

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-leak" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.WaterLeakSensor,
            Properties =
            [
                new WaterLeakProperty { Instance = "water_leak" },
                new BatteryProperty { Instance = "battery" },
            ],
        }],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new WaterLeakState { Instance = "water_leak", Value = leaking },
                new BatteryState { Instance = "battery", Value = battery },
            ],
        });

    // сенсор: команд нет
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);

    private void Report(StateValue value) => Changed?.Invoke(new StateChange { DeviceId = id, Value = value });
}
