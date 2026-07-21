namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущая цветовая температура в кельвинах (instance "temperature_k").</summary>
public sealed record ColorTemperatureState : StateValue
{
    public required int Value { get; init; }
}
