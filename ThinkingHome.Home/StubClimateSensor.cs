using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>
/// Заглушка климатической станции (температура + влажность + заряд батареи, только чтение).
/// Составное устройство по правилу композиции Matter: термометр и гигрометр — независимые
/// прикладные роли, поэтому это два endpoint'а, а не два типа (или безбилетный кластер) на одном.
/// </summary>
public sealed class StubClimateSensor(string id, string title, string? room = null) : IDevice
{
    private readonly double temperature = 23.5;
    private readonly double humidity = 41;
    private readonly double battery = 87;

    public string Id => id;

    public event Action<StateChange>? Changed;

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-climate" },
        Endpoints =
        [
            new Endpoint
            {
                Id = 0,
                Type = DeviceType.TemperatureSensor,
                Properties =
                [
                    new TemperatureProperty { Instance = "temperature" },
                    new BatteryProperty { Instance = "battery" },
                ],
            },
            new Endpoint
            {
                Id = 1,
                Type = DeviceType.HumiditySensor,
                Properties = [new HumidityProperty { Instance = "humidity" }],
            },
        ],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new TemperatureState { EndpointId = 0, Instance = "temperature", Value = temperature },
                new BatteryState { EndpointId = 0, Instance = "battery", Value = battery },
                new HumidityState { EndpointId = 1, Instance = "humidity", Value = humidity },
            ],
        });

    // сенсор: команд нет
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);

    private void Report(StateValue value) => Changed?.Invoke(new StateChange { DeviceId = id, Value = value });
}
