using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ColorModel
{
    [JsonStringEnumMemberName("hsv")] HSV,
    [JsonStringEnumMemberName("rgb")] RGB,
}

public class TemperatureRange
{
    [JsonPropertyName("min")] public int Min { get; set; }
    [JsonPropertyName("max")] public int Max { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ColorSceneId
{
    [JsonStringEnumMemberName("alarm")] Alarm, //Тревога
    [JsonStringEnumMemberName("alice")] Alice, //Алиса
    [JsonStringEnumMemberName("candle")] Candle, //Свеча
    [JsonStringEnumMemberName("dinner")] Dinner, //Ужин
    [JsonStringEnumMemberName("fantasy")] Fantasy, //Фантазия
    [JsonStringEnumMemberName("garland")] Garland, //Гирлянда
    [JsonStringEnumMemberName("jungle")] Jungle, //Джунгли
    [JsonStringEnumMemberName("movie")] Movie, //Кино
    [JsonStringEnumMemberName("neon")] Neon, //Неон
    [JsonStringEnumMemberName("night")] Night, //Ночь
    [JsonStringEnumMemberName("ocean")] Ocean, //Океан
    [JsonStringEnumMemberName("party")] Party, //Вечеринка
    [JsonStringEnumMemberName("reading")] Reading, //Чтение
    [JsonStringEnumMemberName("rest")] Rest, //Отдых
    [JsonStringEnumMemberName("romance")] Romance, //Романтика
    [JsonStringEnumMemberName("siren")] Siren, //Сирена
    [JsonStringEnumMemberName("sunrise")] Sunrise, //Рассвет
    [JsonStringEnumMemberName("sunset")] Sunset, //Закат
}

public class ColorScene
{
    [JsonPropertyName("id")] public ColorSceneId Id { get; set; }
}

public class ColorSceneInfo
{
    [JsonPropertyName("scenes")] public ColorScene[] Scenes { get; set; }
}

public class CapabilityInfoColorSettingParams
{
    [JsonPropertyName("color_model")] public ColorModel ColorModel { get; set; }
    [JsonPropertyName("temperature_k")] public TemperatureRange TemperatureK { get; set; }
    [JsonPropertyName("color_scene")] public ColorSceneInfo ColorScene { get; set; }
}

public class CapabilityInfoColorSetting : CapabilityInfo<CapabilityInfoColorSettingParams>
{
}