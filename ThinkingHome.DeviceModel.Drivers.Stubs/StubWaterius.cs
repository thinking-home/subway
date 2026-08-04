using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Drivers.Stubs;

/// <summary>
/// Заглушка Waterius — двухканального счётчика воды (только чтение). Составное устройство:
/// два одинаковых прибора учёта на двух endpoint'ах. Назначение каналов (холодная/горячая) —
/// монтажная семантика, появится в метаданных endpoint'а.
/// </summary>
public sealed class StubWaterius(string id, StubDeviceConfig config) : IDevice
{
    private readonly double channel0Volume = 123.456;
    private readonly double channel1Volume = 45.678;
    private readonly double battery = 74;

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
        Manufacturer = new DeviceManufacturer { Name = "Waterius", Model = "stub-waterius" },
        Endpoints =
        [
            new Endpoint
            {
                Id = 0,
                Type = DeviceType.WaterMeter,
                Properties =
                [
                    new WaterMeterProperty { Instance = WaterMeterProperty.InstanceName },
                    new BatteryProperty { Instance = BatteryProperty.InstanceName },
                ],
            },
            new Endpoint
            {
                Id = 1,
                Type = DeviceType.WaterMeter,
                Properties = [new WaterMeterProperty { Instance = WaterMeterProperty.InstanceName }],
            },
        ],
    };

    /// <inheritdoc />
    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new WaterMeterState { EndpointId = 0, Instance = WaterMeterProperty.InstanceName, Value = channel0Volume },
                new BatteryState { EndpointId = 0, Instance = BatteryProperty.InstanceName, Value = battery },
                new WaterMeterState { EndpointId = 1, Instance = WaterMeterProperty.InstanceName, Value = channel1Volume },
            ],
        });

    // сенсор: команд нет
    /// <inheritdoc />
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);
}
