using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Properties.Event;
using ThinkingHome.Alice.Model.Properties.Float;

namespace ThinkingHome.Alice.Model.Properties;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PropertyStateFloat), PropertyType.FLOAT)]
[JsonDerivedType(typeof(PropertyStateEvent), PropertyType.EVENT)]
public class PropertyStateBase
{
}

public abstract class PropertyState<TData> : PropertyStateBase
{
    [JsonPropertyName("state")] public TData State { get; set; }
}
