using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel.State;

/// <summary>
/// Текущее значение способности или свойства. Закрытая иерархия, параллельная командам и описаниям.
/// При добавлении наследника обязательно зарегистрировать его в [JsonDerivedType] ниже.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(OnOffState), "onOff")]
[JsonDerivedType(typeof(BrightnessState), "brightness")]
[JsonDerivedType(typeof(ColorTemperatureState), "colorTemperature")]
[JsonDerivedType(typeof(ColorRgbState), "colorRgb")]
[JsonDerivedType(typeof(OpenState), "open")]
public abstract record StateValue
{
    /// <summary>Endpoint устройства (0 — основной).</summary>
    public int EndpointId { get; init; }

    /// <summary>Экземпляр способности/свойства, к которому относится значение.</summary>
    public required string Instance { get; init; }
}
