using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Event;

/// <summary>Состояние событийного свойства: инстанс и значение события.</summary>
public class PropertyStateEventData
{
    /// <summary>Вид отслеживаемого события.</summary>
    [JsonPropertyName("instance")] public PropertyEventInstance Instance { get; set; }

    /// <summary>Текущее значение события.</summary>
    [JsonPropertyName("value")] public PropertyEventValue Value { get; set; }
}
