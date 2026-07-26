using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities.ColorSetting;
using ThinkingHome.Alice.Model.Capabilities.Mode;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.Alice.Model.Capabilities.Range;
using ThinkingHome.Alice.Model.Capabilities.Toggle;

namespace ThinkingHome.Alice.Model.Capabilities;

/// <summary>Базовое состояние способности (query/callback); конкретный тип выбирается по дискриминатору "type" (devices.capabilities.*).</summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CapabilityStateOnOff), CapabilityType.ON_OFF)]
[JsonDerivedType(typeof(CapabilityStateRange), CapabilityType.RANGE)]
[JsonDerivedType(typeof(CapabilityStateColorSetting), CapabilityType.COLOR_SETTING)]
[JsonDerivedType(typeof(CapabilityStateMode), CapabilityType.MODE)]
[JsonDerivedType(typeof(CapabilityStateToggle), CapabilityType.TOGGLE)]
public class CapabilityStateBase
{
}

/// <summary>Состояние способности с данными конкретного вида.</summary>
public abstract class CapabilityState<TParams> : CapabilityStateBase
{
    /// <summary>Состояние способности: инстанс и значение.</summary>
    [JsonPropertyName("state")] public TParams State { get; set; }
}