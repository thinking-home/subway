using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>
/// Временная заглушка лампы (OnOff) с состоянием в памяти. Реальные драйверы придут позже —
/// нужна, чтобы поднять домашний хост и проверить сквозной путь до Алисы.
/// </summary>
public sealed class StubLamp(string id, string title, string? room = null) : IDevice
{
    private bool isOn;

    public string Id => id;

    public event Action<StateChange>? Changed;

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-lamp" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.OnOffLight,
            Capabilities = [new OnOffCapability { Instance = "on" }],
        }],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values = [new OnOffState { Instance = "on", Value = isOn }],
        });

    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
    {
        if (command is OnOffCommand cmd)
        {
            isOn = cmd.Value;
            Console.WriteLine($"[{id}] → {(isOn ? "ВКЛ" : "выкл")}");
            Changed?.Invoke(new StateChange
            {
                DeviceId = id,
                Value = new OnOffState { Instance = "on", Value = isOn },
            });
            return Task.FromResult(CommandOutcome.Done);
        }

        return Task.FromResult(CommandOutcome.Unsupported);
    }
}
