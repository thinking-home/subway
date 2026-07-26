namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Включить/выключить осцилляцию (instance "oscillation").</summary>
public sealed record OscillationCommand : DeviceCommand
{
    /// <summary>Целевое состояние осцилляции: true — включить.</summary>
    public required bool Value { get; init; }
}
