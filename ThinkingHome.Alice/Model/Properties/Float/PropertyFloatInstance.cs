using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Float;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertyFloatInstance
{
    [JsonStringEnumMemberName("temperature")] Temperature,
    [JsonStringEnumMemberName("humidity")] Humidity,
    [JsonStringEnumMemberName("battery_level")] BatteryLevel,
}
