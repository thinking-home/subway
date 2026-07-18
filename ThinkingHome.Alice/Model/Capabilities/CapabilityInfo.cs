using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.Alice.Model.Capabilities.Range;

namespace ThinkingHome.Alice.Model.Capabilities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CapabilityInfoOnOff), CapabilityType.ON_OFF)]
[JsonDerivedType(typeof(CapabilityInfoRange), CapabilityType.RANGE)]
public class CapabilityInfoBase
{
    [JsonPropertyName("retrievable")] public bool Retrievable { get; set; }


    [JsonPropertyName("reportable")] public bool Reportable { get; set; }
}

public abstract class CapabilityInfo<TParams> : CapabilityInfoBase
{
    [JsonPropertyName("parameters")] public TParams Parameters { get; set; }
}