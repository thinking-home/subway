using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities.OnOff;

namespace ThinkingHome.Alice.Model.Capabilities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CapabilityStateOnOff), CapabilityType.ON_OFF)]
public class CapabilityStateBase
{
}

public abstract class CapabilityState<TParams> : CapabilityStateBase
{
    [JsonPropertyName("state")] public TParams State { get; set; }
}