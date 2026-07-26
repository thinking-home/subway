namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая концентрация CO2, ppm (instance "carbon_dioxide").</summary>
public sealed record CarbonDioxideState : StateValue
{
    /// <summary>Концентрация CO2, ppm.</summary>
    public required double Value { get; init; }
}
