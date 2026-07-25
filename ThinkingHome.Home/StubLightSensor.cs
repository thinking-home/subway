using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>Заглушка датчика освещённости (только чтение).</summary>
public sealed class StubLightSensor(string id, string title, string? room = null) : IDevice
{
    private readonly double lux = 420;
    private readonly double battery = 78;

    public string Id => id;

    public event Action<StateChange>? Changed;

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-light-sensor" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.LightSensor,
            Properties =
            [
                new IlluminanceProperty { Instance = "illuminance" },
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
                new IlluminanceState { Instance = "illuminance", Value = lux },
                new BatteryState { Instance = "battery", Value = battery },
            ],
        });

    // сенсор: команд нет
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);
}
