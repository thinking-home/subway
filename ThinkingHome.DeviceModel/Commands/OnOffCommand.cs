namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Включить/выключить (instance "on").</summary>
public sealed record OnOffCommand : DeviceCommand
{
    /// <summary>Целевое состояние: true — включить.</summary>
    public required bool Value { get; init; }
}
