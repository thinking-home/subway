using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>
/// Заглушка шторы — одно свойство «положение» 0–100 % (0 — закрыта, 100 — открыта), как Window Covering
/// (0x0102) в Matter. Принимает две команды: OpenCommand (в положение %) и OnOffCommand (открыть/закрыть =
/// крайние положения, аналог Matter UpOrOpen/DownOrClose). Тумблер on_off для Алисы (умение + состояние)
/// синтезирует маппер — в ядре отдельного on/off-состояния нет.
/// </summary>
public sealed class StubCurtain(string id, string title, string? room = null) : IDevice
{
    private int position; // 0 — закрыта, 100 — открыта

    public string Id => id;

    public event Action<StateChange>? Changed;

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-curtain" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Types = [DeviceType.Curtain],
            Capabilities = [new OpenCapability { Instance = "open" }],
        }],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values = [new OpenState { Instance = "open", Value = position }],
        });

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
        Changed?.Invoke(new StateChange { DeviceId = id, Value = new OpenState { Instance = "open", Value = position } });
        return Task.FromResult(CommandOutcome.Done);
    }
}
