namespace ThinkingHome.DeviceModel.Capabilities;

/// <summary>
/// Цветовая температура в кельвинах. Matter cluster Color Control (0x0300). Instance — "temperature_k".
/// Диапазон зависит от устройства (по умолчанию 2700–6500 K).
/// </summary>
public sealed record ColorTemperatureCapability : Capability
{
    public int MinKelvin { get; init; } = 2700;
    public int MaxKelvin { get; init; } = 6500;
}
