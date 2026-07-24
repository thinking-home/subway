using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>Заглушка климатического датчика (температура + влажность + заряд батареи, только чтение).</summary>
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
        Endpoints = [new Endpoint
        {
            Id = 0,
            Types = [DeviceType.TemperatureSensor],
            Properties =
            [
                new TemperatureProperty { Instance = "temperature" },
                new HumidityProperty { Instance = "humidity" },
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
                new TemperatureState { Instance = "temperature", Value = temperature },
                new HumidityState { Instance = "humidity", Value = humidity },
                new BatteryState { Instance = "battery", Value = battery },
            ],
        });

    // сенсор: команд нет
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);

    private void Report(StateValue value) => Changed?.Invoke(new StateChange { DeviceId = id, Value = value });
}
