namespace ThinkingHome.DeviceModel.Capabilities;

/// <summary>
/// Цвет: полноцветный режим и/или цветовая температура — одна способность, как кластер Color Control
/// (0x0300) в Matter. Представления цвета в состояниях/командах (<c>ColorRgbState</c>,
/// <c>ColorTemperatureState</c> и т.п.) делят <see cref="InstanceName"/>: это один логический признак с
/// взаимоисключающими представлениями (тип = представление), поэтому в кэше у них общий слот и
/// переключение режима перезаписывает значение, а не накапливает.
/// </summary>
public sealed record ColorCapability : Capability
{
    /// <summary>Единый Instance всех цветовых способностей/команд/состояний (общий слот кэша).</summary>
    public const string InstanceName = "color";

    /// <summary>Модель полного цвета; null — полный цвет не поддерживается.</summary>
    public ColorModel? Model { get; init; }

    /// <summary>Диапазон цветовой температуры; null — температура не поддерживается.</summary>
    public ColorTemperatureRange? Temperature { get; init; }
}

/// <summary>Диапазон цветовой температуры в кельвинах (границы задаются только вместе).</summary>
public sealed record ColorTemperatureRange
{
    public required int MinKelvin { get; init; }
    public required int MaxKelvin { get; init; }
}
