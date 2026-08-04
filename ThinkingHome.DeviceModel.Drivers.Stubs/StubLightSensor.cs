using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Drivers.Stubs;

/// <summary>Заглушка датчика освещённости (только чтение).</summary>
public sealed class StubLightSensor(string id, StubDeviceConfig config) : IDevice
{
    private readonly double lux = 420;
    private readonly double battery = 78;

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
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-light-sensor" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.LightSensor,
            Properties =
            [
                new IlluminanceProperty { Instance = IlluminanceProperty.InstanceName },
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
                new IlluminanceState { Instance = IlluminanceProperty.InstanceName, Value = lux },
                new BatteryState { Instance = BatteryProperty.InstanceName, Value = battery },
            ],
        });

    // сенсор: команд нет
    /// <inheritdoc />
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);
}
