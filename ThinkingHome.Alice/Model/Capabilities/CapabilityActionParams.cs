using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities.OnOff;

namespace ThinkingHome.Alice.Model.Capabilities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CapabilityActionParamsOnOff), CapabilityType.ON_OFF)]
public class CapabilityActionParamsBase
{
}

public abstract class CapabilityActionParams<TParams> : CapabilityActionParamsBase
{
    [JsonPropertyName("state")] public TParams State { get; set; }
}