using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>
/// Заглушка Waterius — двухканального счётчика воды (только чтение). Составное устройство:
/// два одинаковых прибора учёта на двух endpoint'ах. Назначение каналов (холодная/горячая) —
/// монтажная семантика, появится в метаданных endpoint'а.
/// </summary>
public sealed class StubWaterius(string id, string title, string? room = null) : IDevice
{
    private readonly double channel0Volume = 123.456;
    private readonly double channel1Volume = 45.678;
    private readonly double battery = 74;

    public string Id => id;

    // статический стаб: изменений не эмитит
    public event Action<StateChange>? Changed { add { } remove { } }

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "Waterius", Model = "stub-waterius" },
        Endpoints =
        [
            new Endpoint
            {
                Id = 0,
                Type = DeviceType.WaterMeter,
                Properties =
                [
                    new WaterMeterProperty { Instance = "water_meter" },
                    new BatteryProperty { Instance = "battery" },
                ],
            },
            new Endpoint
            {
                Id = 1,
                Type = DeviceType.WaterMeter,
                Properties = [new WaterMeterProperty { Instance = "water_meter" }],
            },
        ],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new WaterMeterState { EndpointId = 0, Instance = "water_meter", Value = channel0Volume },
                new BatteryState { EndpointId = 0, Instance = "battery", Value = battery },
                new WaterMeterState { EndpointId = 1, Instance = "water_meter", Value = channel1Volume },
            ],
        });

    // сенсор: команд нет
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);
}
