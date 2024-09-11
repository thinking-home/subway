using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CapabilityInfoOnOff), CapabilityType.ON_OFF)]
[JsonDerivedType(typeof(CapabilityInfoColorSetting), CapabilityType.COLOR_SETTING)]
public class CapabilityInfoBase
{
    public bool retrievable { get; set; }
    public bool reportable { get; set; }
}


public abstract class CapabilityInfo<TParams>: CapabilityInfoBase
{
    public TParams parameters { get; set; }
}
