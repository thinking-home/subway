namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая цветовая температура в кельвинах (instance "color").</summary>
public sealed record ColorTemperatureState : StateValue
{
    /// <summary>Цветовая температура, K.</summary>
    public required int Value { get; init; }
}
