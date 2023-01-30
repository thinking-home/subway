using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CapabilityInfoOnOff), CapabilityType.TYPE_ON_OFF)]
[JsonDerivedType(typeof(CapabilityInfoColorSetting), CapabilityType.TYPE_COLOR_SETTING)]
public class CapabilityInfoBase
{
    public bool retrievable { get; set; }
    public bool reportable { get; set; }
}


public abstract class CapabilityInfo<TParams>: CapabilityInfoBase
{
    public TParams parameters { get; set; }
}
