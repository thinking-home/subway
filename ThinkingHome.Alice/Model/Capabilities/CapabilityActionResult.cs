using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.ActionResult;
using ThinkingHome.Alice.Model.Capabilities.ColorSetting;
using ThinkingHome.Alice.Model.Capabilities.Mode;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.Alice.Model.Capabilities.Range;
using ThinkingHome.Alice.Model.Capabilities.Toggle;

namespace ThinkingHome.Alice.Model.Capabilities;

/// <summary>Базовый класс результата операции над способностью в ответе на action; конкретный тип выбирается по дискриминатору "type" (devices.capabilities.*).</summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CapabilityActionResultOnOff), CapabilityType.ON_OFF)]
[JsonDerivedType(typeof(CapabilityActionResultRange), CapabilityType.RANGE)]
[JsonDerivedType(typeof(CapabilityActionResultColorSetting), CapabilityType.COLOR_SETTING)]
[JsonDerivedType(typeof(CapabilityActionResultMode), CapabilityType.MODE)]
[JsonDerivedType(typeof(CapabilityActionResultToggle), CapabilityType.TOGGLE)]
public class CapabilityActionResultBase
{
}

/// <summary>Результат операции над способностью с инстансом конкретного вида.</summary>
public class CapabilityActionResult<TInstance> : CapabilityActionResultBase
{
    /// <summary>Ответ с результатом операции над конкретным умением.</summary>
    [JsonPropertyName("state")] public CapabilityStateActionResult<TInstance> State { get; set; }
}
