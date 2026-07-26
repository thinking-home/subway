using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Event;

/// <summary>Параметры событийного свойства в discovery: инстанс и поддерживаемые события.</summary>
public class PropertyEventParams
{
    /// <summary>Вид отслеживаемого события.</summary>
    [JsonPropertyName("instance")] public PropertyEventInstance Instance { get; set; }

    /// <summary>Поддерживаемые значения события.</summary>
    [JsonPropertyName("events")] public PropertyEventOption[] Events { get; set; }
}

/// <summary>Одно поддерживаемое значение события в параметрах свойства.</summary>
public class PropertyEventOption
{
    /// <summary>Значение события из словаря Алисы.</summary>
    [JsonPropertyName("value")] public PropertyEventValue Value { get; set; }
}
