using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>Заглушка датчика открытия (только чтение). true — контакт замкнут (закрыто).</summary>
public sealed class StubContactSensor(string id, string title, string? room = null) : IDevice
{
    private readonly bool contact = true;

    public string Id => id;

    // статический стаб: изменений не эмитит
    public event Action<StateChange>? Changed { add { } remove { } }

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-contact" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.ContactSensor,
            Properties = [new ContactProperty { Instance = ContactProperty.InstanceName }],
        }],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values = [new ContactState { Instance = ContactProperty.InstanceName, Value = contact }],
        });

    // сенсор: команд нет
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        => Task.FromResult(CommandOutcome.Unsupported);
}
