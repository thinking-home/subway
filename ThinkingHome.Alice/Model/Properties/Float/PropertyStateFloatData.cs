using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Float;

/// <summary>Состояние числового свойства: инстанс и значение.</summary>
public class PropertyStateFloatData
{
    /// <summary>Измеряемая величина.</summary>
    [JsonPropertyName("instance")] public PropertyFloatInstance Instance { get; set; }

    /// <summary>Текущее значение.</summary>
    [JsonPropertyName("value")] public float Value { get; set; }
}
