using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Float;

/// <summary>Параметры числового свойства в discovery: инстанс и единица измерения.</summary>
public class PropertyFloatParams
{
    /// <summary>Измеряемая величина.</summary>
    [JsonPropertyName("instance")] public PropertyFloatInstance Instance { get; set; }

    /// <summary>Единица измерения (константа из <see cref="Capabilities.Units"/>).</summary>
    [JsonPropertyName("unit")] public string Unit { get; set; }
}
