using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ColorModel
{
    hsv,
    rgb,
}

public class TemperatureRange
{
    public int min { get; set; }
    public int max { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ColorSceneId
{
    alarm, //Тревога
    alice, //Алиса
    candle, //Свеча
    dinner, //Ужин
    fantasy, //Фантазия
    garland, //Гирлянда
    jungle, //Джунгли
    movie, //Кино
    neon, //Неон
    night, //Ночь
    ocean, //Океан
    party, //Вечеринка
    reading, //Чтение
    rest, //Отдых
    romance, //Романтика
    siren, //Сирена
    sunrise, //Рассвет
    sunset, //Закат
}

public class ColorScene
{
    public ColorSceneId id { get; set; }
}

public class ColorSceneInfo
{
    public ColorScene[] scenes { get; set; }
}

public class CapabilityInfoColorSettingParams
{
    public ColorModel color_model { get; set; }
    public TemperatureRange temperature_k { get; set; }
    public ColorSceneInfo color_scene { get; set; }
}

public class CapabilityInfoColorSetting : CapabilityInfo<CapabilityInfoColorSettingParams>
{
}
