namespace ThinkingHome.DeviceModel.Properties;

/// <summary>Освещённость, лк — нормализованная единица ядра (Matter хранит 10000·log10(lux)+1, нормализует драйвер). Matter cluster Illuminance Measurement (0x0400). Instance — "illuminance".</summary>
public sealed record IlluminanceProperty : Property
{
    /// <summary>Канонический instance.</summary>
    public const string InstanceName = "illuminance";
}
