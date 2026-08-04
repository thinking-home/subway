namespace ThinkingHome.DeviceModel.Drivers.Stubs;

/// <summary>Настройки стабового устройства.</summary>
public class StubDeviceConfig
{
    /// <summary>Человекочитаемое название устройства.</summary>
    public string Title { get; set; } = "";

    /// <summary>Комната, где «установлено» устройство.</summary>
    public string? Room { get; set; }
}

/// <summary>Настройки стабового вкл/выкл-устройства: дополнительно тип прибора.</summary>
public sealed class StubOnOffDeviceConfig : StubDeviceConfig
{
    /// <summary>Тип прибора: лампа, розетка или выключатель.</summary>
    public DeviceType Type { get; set; } = DeviceType.OnOffLight;
}
