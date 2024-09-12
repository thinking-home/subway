using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CapabilityInfoOnOff), CapabilityType.ON_OFF)]
[JsonDerivedType(typeof(CapabilityInfoColorSetting), CapabilityType.COLOR_SETTING)]
public class CapabilityInfoBase
{
    [JsonPropertyName("retrievable")] public bool Retrievable { get; set; }


    [JsonPropertyName("reportable")] public bool Reportable { get; set; }
}

public abstract class CapabilityInfo<TParams> : CapabilityInfoBase
{
    [JsonPropertyName("parameters")] public TParams Parameters { get; set; }
}