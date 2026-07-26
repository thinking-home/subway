using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities.ColorSetting;
using ThinkingHome.Alice.Model.Capabilities.Mode;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.Alice.Model.Capabilities.Range;
using ThinkingHome.Alice.Model.Capabilities.Toggle;

namespace ThinkingHome.Alice.Model.Capabilities;

/// <summary>Базовое описание способности в discovery; конкретный тип выбирается по дискриминатору "type" (devices.capabilities.*).</summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CapabilityInfoOnOff), CapabilityType.ON_OFF)]
[JsonDerivedType(typeof(CapabilityInfoRange), CapabilityType.RANGE)]
[JsonDerivedType(typeof(CapabilityInfoColorSetting), CapabilityType.COLOR_SETTING)]
[JsonDerivedType(typeof(CapabilityInfoMode), CapabilityType.MODE)]
[JsonDerivedType(typeof(CapabilityInfoToggle), CapabilityType.TOGGLE)]
public class CapabilityInfoBase
{
    /// <summary>Доступно ли чтение состояния способности (запросы query).</summary>
    [JsonPropertyName("retrievable")] public bool Retrievable { get; set; }


    /// <summary>Сообщает ли устройство об изменении состояния через Notification API.</summary>
    [JsonPropertyName("reportable")] public bool Reportable { get; set; }
}

/// <summary>Описание способности с параметрами конкретного вида.</summary>
public abstract class CapabilityInfo<TParams> : CapabilityInfoBase
{
    /// <summary>Параметры способности.</summary>
    [JsonPropertyName("parameters")] public TParams Parameters { get; set; }
}