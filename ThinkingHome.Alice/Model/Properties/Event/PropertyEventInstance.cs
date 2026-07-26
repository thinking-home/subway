using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Event;

/// <summary>Инстанс событийного свойства — вид отслеживаемого события.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertyEventInstance
{
    /// <summary>Движение.</summary>
    [JsonStringEnumMemberName("motion")] Motion,
    /// <summary>Открытие двери/окна.</summary>
    [JsonStringEnumMemberName("open")] Open,
    /// <summary>Протечка воды.</summary>
    [JsonStringEnumMemberName("water_leak")] WaterLeak,
}
