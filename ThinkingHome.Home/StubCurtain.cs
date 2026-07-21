using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>
/// Заглушка шторы (открыть/закрыть + положение). Держит одно число — положение 0–100 % (0 — закрыта,
/// 100 — открыта); on_off задаёт крайние положения, range "open" — произвольное.
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
            Type = DeviceType.Curtain,
            Capabilities = [new OnOffCapability { Instance = "on" }, new OpenCapability { Instance = "open" }],
        }],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new OnOffState { Instance = "on", Value = position > 0 },
                new OpenState { Instance = "open", Value = position },
            ],
        });

    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
    {
        switch (command)
        {
            case OnOffCommand on:
                position = on.Value ? 100 : 0;
                Console.WriteLine($"[{id}] → {(on.Value ? "открыть" : "закрыть")} ({position}%)");
                ReportPosition();
                return Task.FromResult(CommandOutcome.Done);

            case OpenCommand open:
                position = open.Value;
                Console.WriteLine($"[{id}] → положение {position}%");
                ReportPosition();
                return Task.FromResult(CommandOutcome.Done);

            default:
                return Task.FromResult(CommandOutcome.Unsupported);
        }
    }

    // положение отражается сразу в двух инстансах (on/open) — репортим оба, чтобы кэш не рассинхронился
    private void ReportPosition()
    {
        Report(new OnOffState { Instance = "on", Value = position > 0 });
        Report(new OpenState { Instance = "open", Value = position });
    }

    private void Report(StateValue value) => Changed?.Invoke(new StateChange { DeviceId = id, Value = value });
}
