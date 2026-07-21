using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel;

/// <summary>
/// Модель полного цвета. Модели взаимоисключающие — устройство поддерживает одну; пока только RGB.
///
/// Как добавить HSV: сюда — значение <c>Hsv</c>; в State/Commands завести подтипы
/// <c>ColorHsvState</c>/<c>ColorHsvCommand</c> с полями { H, S, V } (значение не число, поэтому
/// отдельный тип, а не поле в существующем); на стороне Алисы значение hsv у color_setting — объект
/// {h,s,v}, поэтому Alice-DTO цвета придётся разветвить (скаляр для rgb/temperature_k vs объект hsv).
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ColorModel
{
    Rgb,
}
