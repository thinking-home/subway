using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Drivers.Stubs;

/// <summary>
/// Заглушка шторы — одно свойство «положение» 0–100 % (0 — закрыта, 100 — открыта), как Window Covering
/// (0x0102) в Matter. Принимает две команды: OpenCommand (в положение %) и OnOffCommand (открыть/закрыть =
/// крайние положения, аналог Matter UpOrOpen/DownOrClose). Тумблер on_off для Алисы (умение + состояние)
/// синтезирует маппер — в ядре отдельного on/off-состояния нет.
/// </summary>
public sealed class StubCurtain(string id, StubDeviceConfig config) : IDevice
{
    private int position; // 0 — закрыта, 100 — открыта

    /// <inheritdoc />
    public string Id => id;

    /// <inheritdoc />
    public event Action<StateChange>? Changed;

    /// <inheritdoc />
    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = config.Title,
        Room = config.Room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-curtain" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.Curtain,
            Capabilities = [new OpenCapability { Instance = OpenCapability.InstanceName }],
        }],
    };

    /// <inheritdoc />
    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values = [new OpenState { Instance = OpenCapability.InstanceName, Value = position }],
        });

    /// <inheritdoc />
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
    {
        switch (command)
        {
            case OpenCommand open:
                position = open.Value;
                break;
            case OnOffCommand on: // открыть/закрыть = крайние положения
                position = on.Value ? 100 : 0;
                break;
            default:
                return Task.FromResult(CommandOutcome.Unsupported);
        }

        Console.WriteLine($"[{id}] → положение {position}%");
        Changed?.Invoke(new StateChange { DeviceId = id, Value = new OpenState { Instance = OpenCapability.InstanceName, Value = position } });
        return Task.FromResult(CommandOutcome.Done);
    }
}
