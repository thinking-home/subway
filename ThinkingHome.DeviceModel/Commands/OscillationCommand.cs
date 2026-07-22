namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Включить/выключить осцилляцию (instance "oscillation").</summary>
public sealed record OscillationCommand : DeviceCommand
{
    public required bool Value { get; init; }
}
