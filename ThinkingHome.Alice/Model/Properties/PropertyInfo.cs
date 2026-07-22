using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Properties.Event;
using ThinkingHome.Alice.Model.Properties.Float;

namespace ThinkingHome.Alice.Model.Properties;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PropertyInfoFloat), PropertyType.FLOAT)]
[JsonDerivedType(typeof(PropertyInfoEvent), PropertyType.EVENT)]
public class PropertyInfoBase
{
    [JsonPropertyName("retrievable")] public bool Retrievable { get; set; }

    [JsonPropertyName("reportable")] public bool Reportable { get; set; }
}

public abstract class PropertyInfo<TParams> : PropertyInfoBase
{
    [JsonPropertyName("parameters")] public TParams Parameters { get; set; }
}
