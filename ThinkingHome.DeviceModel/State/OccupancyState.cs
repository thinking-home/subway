namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущее присутствие/движение (instance "occupancy"). true — движение обнаружено.</summary>
public sealed record OccupancyState : StateValue
{
    /// <summary>Присутствие/движение обнаружено.</summary>
    public required bool Value { get; init; }
}
