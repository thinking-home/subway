namespace ThinkingHome.Alice.Model.Capabilities;

/// <summary>
/// Единицы измерения Алисы (поле "unit" у range/float). Единицу выбирает конкретная способность:
/// яркость всегда в процентах. По мере добавления инстансов (температура, громкость) сюда добавляются
/// новые константы, а нужную подставляет соответствующая ветка маппера.
/// </summary>
public static class Units
{
    /// <summary>Проценты.</summary>
    public const string PERCENT = "unit.percent";
    /// <summary>Градусы Цельсия.</summary>
    public const string CELSIUS = "unit.temperature.celsius";
    /// <summary>Люксы (освещённость).</summary>
    public const string LUX = "unit.illumination.lux";
    /// <summary>Миллиметры ртутного столба (давление).</summary>
    public const string MMHG = "unit.pressure.mmhg";
    /// <summary>Части на миллион (концентрация газа).</summary>
    public const string PPM = "unit.ppm";
    /// <summary>Кубические метры.</summary>
    public const string CUBIC_METER = "unit.cubic_meter";
}
