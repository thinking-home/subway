using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.ActionResult;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.Alice.Model.Capabilities.Range;

namespace ThinkingHome.Alice.Model.Capabilities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CapabilityActionResultOnOff), CapabilityType.ON_OFF)]
[JsonDerivedType(typeof(CapabilityActionResultRange), CapabilityType.RANGE)]
public class CapabilityActionResultBase
{
}

public class CapabilityActionResult<TInstance> : CapabilityActionResultBase
{
    // ответ с результатом операции над конкретным умением
    [JsonPropertyName("state")] public CapabilityStateActionResult<TInstance> State { get; set; }
}
