using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities.ColorSetting;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.Alice.Model.Capabilities.Range;

namespace ThinkingHome.Alice.Model.Capabilities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CapabilityStateOnOff), CapabilityType.ON_OFF)]
[JsonDerivedType(typeof(CapabilityStateRange), CapabilityType.RANGE)]
[JsonDerivedType(typeof(CapabilityStateColorSetting), CapabilityType.COLOR_SETTING)]
public class CapabilityStateBase
{
}

public abstract class CapabilityState<TParams> : CapabilityStateBase
{
    [JsonPropertyName("state")] public TParams State { get; set; }
}