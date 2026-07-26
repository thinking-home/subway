namespace ThinkingHome.Alice.Model.Capabilities;

/// <summary>Строковые идентификаторы типов способностей Алисы (devices.capabilities.*) — значения дискриминатора "type".</summary>
public static class CapabilityType
{
    /// <summary>Включение и выключение.</summary>
    public const string ON_OFF = "devices.capabilities.on_off";
    /// <summary>Управление цветом и цветовой температурой.</summary>
    public const string COLOR_SETTING = "devices.capabilities.color_setting";
    /// <summary>Дискретные режимы работы.</summary>
    public const string MODE = "devices.capabilities.mode";
    /// <summary>Параметр в числовом диапазоне.</summary>
    public const string RANGE = "devices.capabilities.range";
    /// <summary>Переключаемая дополнительная функция (вкл/выкл).</summary>
    public const string TOGGLE = "devices.capabilities.toggle";
}
