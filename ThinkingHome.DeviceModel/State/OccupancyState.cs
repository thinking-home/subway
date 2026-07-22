namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущее присутствие/движение (instance "occupancy"). true — движение обнаружено.</summary>
public sealed record OccupancyState : StateValue
{
    public required bool Value { get; init; }
}
