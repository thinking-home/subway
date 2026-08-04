using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Drivers.Stubs;

/// <summary>Заглушка датчика протечки (только чтение). true — протечка обнаружена.</summary>
public sealed class StubWaterLeakSensor(string id, StubDeviceConfig config) : IDevice
{
    private readonly bool leaking = false;
    private readonly double battery = 92;

    /// <inheritdoc />
    public string Id => id;

    // статический стаб: изменений не эмитит
    /// <inheritdoc />
    public event Action<StateChange>? Changed { add { } remove { } }

    /// <inheritdoc />
    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = config.Title,
        Room = config.Room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-leak" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.WaterLeakSensor,
            Properties =
            [
                new WaterLeakProperty { Instance = WaterLeakProperty.InstanceName },
                new BatteryProperty { Instance = BatteryProperty.InstanceName },
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
                new WaterLeakState { Instance = WaterLeakProperty.InstanceName, Value = leaking },
                new BatteryState { Instance = BatteryProperty.InstanceName, Value = battery },
            ],
        });

    // сенсор: команд нет
    /// <inheritdoc />
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);
}
