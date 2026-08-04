using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Drivers.Stubs;

/// <summary>Заглушка датчика движения (только чтение).</summary>
public sealed class StubMotionSensor(string id, StubDeviceConfig config) : IDevice
{
    private readonly bool occupied = true;

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
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-motion" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.OccupancySensor,
            Properties = [new OccupancyProperty { Instance = OccupancyProperty.InstanceName }],
        }],
    };

    /// <inheritdoc />
    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values = [new OccupancyState { Instance = OccupancyProperty.InstanceName, Value = occupied }],
        });

    // сенсор: команд нет
    /// <inheritdoc />
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);
}
