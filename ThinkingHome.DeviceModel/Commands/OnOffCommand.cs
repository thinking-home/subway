namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Включить/выключить (instance "on").</summary>
public sealed record OnOffCommand : DeviceCommand
{
    public required bool Value { get; init; }
}
