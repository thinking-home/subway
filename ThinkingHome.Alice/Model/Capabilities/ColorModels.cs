namespace ThinkingHome.Alice.Model.Capabilities;

/// <summary>
/// Цветовые модели Алисы (поле "color_model" в color_setting). У Алисы их две — "rgb" и "hsv", но это
/// взаимозаменяемые представления одного цвета: устройство объявляет одну. Берём "rgb" (значение — одно
/// число 0xRRGGBB). "hsv" намеренно не реализован: функционально эквивалентен rgb, но его значение —
/// объект {h,s,v}, что потребовало бы отдельной формы состояния без пользы для управления.
/// </summary>
public static class ColorModels
{
    public const string RGB = "rgb";
}
