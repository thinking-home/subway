using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel.Commands;

/// <summary>
/// Команда изменения состояния способности. Закрытая иерархия: тип-дискриминатор + родное значение.
/// Драйвер разбирает команду через pattern matching (switch по типу).
/// При добавлении наследника обязательно зарегистрировать его в [JsonDerivedType] ниже.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(OnOffCommand), "onOff")]
[JsonDerivedType(typeof(BrightnessCommand), "brightness")]
[JsonDerivedType(typeof(ColorTemperatureCommand), "colorTemperature")]
[JsonDerivedType(typeof(ColorRgbCommand), "colorRgb")]
[JsonDerivedType(typeof(OpenCommand), "open")]
public abstract record DeviceCommand
{
    /// <summary>Endpoint устройства (0 — основной).</summary>
    public int EndpointId { get; init; }

    /// <summary>Экземпляр способности, к которой относится команда.</summary>
    public required string Instance { get; init; }
}
