using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.OnOff;

/// <summary>Параметры on_off в discovery.</summary>
public class CapabilityInfoOnOffParams
{
    /// <summary>Раздельные команды включения и выключения (без чтения текущего состояния).</summary>
    [JsonPropertyName("split")] public bool Split { get; set; }
}