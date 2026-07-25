namespace ThinkingHome.Alice.Model.Capabilities;

/// <summary>
/// Единицы измерения Алисы (поле "unit" у range/float). Единицу выбирает конкретная способность:
/// яркость всегда в процентах. По мере добавления инстансов (температура, громкость) сюда добавляются
/// новые константы, а нужную подставляет соответствующая ветка маппера.
/// </summary>
public static class Units
{
    public const string PERCENT = "unit.percent";
    public const string CELSIUS = "unit.temperature.celsius";
    public const string LUX = "unit.illumination.lux";
    public const string MMHG = "unit.pressure.mmhg";
    public const string PPM = "unit.ppm";
    public const string CUBIC_METER = "unit.cubic_meter";
}
