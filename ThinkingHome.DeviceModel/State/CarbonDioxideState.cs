namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая концентрация CO2, ppm (instance "carbon_dioxide").</summary>
public sealed record CarbonDioxideState : StateValue
{
    public required double Value { get; init; }
}
