using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Float;

/// <summary>Инстанс числового свойства — измеряемая величина.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertyFloatInstance
{
    /// <summary>Температура.</summary>
    [JsonStringEnumMemberName("temperature")] Temperature,
    /// <summary>Влажность.</summary>
    [JsonStringEnumMemberName("humidity")] Humidity,
    /// <summary>Уровень заряда батареи.</summary>
    [JsonStringEnumMemberName("battery_level")] BatteryLevel,
    /// <summary>Освещённость.</summary>
    [JsonStringEnumMemberName("illumination")] Illumination,
    /// <summary>Давление.</summary>
    [JsonStringEnumMemberName("pressure")] Pressure,
    /// <summary>Уровень углекислого газа.</summary>
    [JsonStringEnumMemberName("co2_level")] Co2Level,
    /// <summary>Показание счётчика воды.</summary>
    [JsonStringEnumMemberName("water_meter")] WaterMeter,
}
