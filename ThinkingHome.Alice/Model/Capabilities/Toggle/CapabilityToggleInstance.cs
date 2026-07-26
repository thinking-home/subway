using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities.Toggle;

/// <summary>Инстанс способности toggle — какая функция устройства переключается.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CapabilityToggleInstance
{
    /// <summary>Осцилляция (вращение корпуса вентилятора).</summary>
    [JsonStringEnumMemberName("oscillation")] Oscillation,
}
