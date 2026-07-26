using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>
/// Временная заглушка диммируемой лампы (OnOff + яркость) с состоянием в памяти. Реальные драйверы
/// придут позже; нужна, чтобы проверить сквозной путь новой способности «яркость» до Алисы.
/// </summary>
public sealed class StubDimmableLamp(string id, string title, string? room = null) : IDevice
{
    private bool isOn;
    private int brightness = 100;

    public string Id => id;

    public event Action<StateChange>? Changed;

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-dimmable" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.DimmableLight,
            Capabilities = [new OnOffCapability { Instance = OnOffCapability.InstanceName }, new BrightnessCapability { Instance = BrightnessCapability.InstanceName }],
        }],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new OnOffState { Instance = OnOffCapability.InstanceName, Value = isOn },
                new BrightnessState { Instance = BrightnessCapability.InstanceName, Value = brightness },
            ],
        });

    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
    {
        switch (command)
        {
            case OnOffCommand on:
                isOn = on.Value;
                Console.WriteLine($"[{id}] → {(isOn ? "ВКЛ" : "выкл")}");
                Report(new OnOffState { Instance = OnOffCapability.InstanceName, Value = isOn });
                return Task.FromResult(CommandOutcome.Done);

            case BrightnessCommand br:
                brightness = br.Value;
                Console.WriteLine($"[{id}] → яркость {brightness}%");
                Report(new BrightnessState { Instance = BrightnessCapability.InstanceName, Value = brightness });
                return Task.FromResult(CommandOutcome.Done);

            default:
                return Task.FromResult(CommandOutcome.Unsupported);
        }
    }

    private void Report(StateValue value) => Changed?.Invoke(new StateChange { DeviceId = id, Value = value });
}
