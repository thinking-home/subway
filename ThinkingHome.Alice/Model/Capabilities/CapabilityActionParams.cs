using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities.ColorSetting;
using ThinkingHome.Alice.Model.Capabilities.Mode;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.Alice.Model.Capabilities.Range;
using ThinkingHome.Alice.Model.Capabilities.Toggle;

namespace ThinkingHome.Alice.Model.Capabilities;

/// <summary>Базовый класс параметров способности в action-запросе; конкретный тип выбирается по дискриминатору "type" (devices.capabilities.*).</summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CapabilityActionParamsOnOff), CapabilityType.ON_OFF)]
[JsonDerivedType(typeof(CapabilityActionParamsRange), CapabilityType.RANGE)]
[JsonDerivedType(typeof(CapabilityActionParamsColorSetting), CapabilityType.COLOR_SETTING)]
[JsonDerivedType(typeof(CapabilityActionParamsMode), CapabilityType.MODE)]
[JsonDerivedType(typeof(CapabilityActionParamsToggle), CapabilityType.TOGGLE)]
public class CapabilityActionParamsBase
{
}

/// <summary>Параметры способности в action-запросе с целевым состоянием конкретного вида.</summary>
public abstract class CapabilityActionParams<TParams> : CapabilityActionParamsBase
{
    /// <summary>Целевое состояние способности.</summary>
    [JsonPropertyName("state")] public TParams State { get; set; }
}