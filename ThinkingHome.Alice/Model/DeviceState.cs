using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities;
using ThinkingHome.Alice.Model.Properties;

namespace ThinkingHome.Alice.Model;

public class DeviceState
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("capabilities")] public CapabilityStateBase[] Capabilities { get; set; }
    [JsonPropertyName("properties")] public PropertyStateBase[] Properties { get; set; }

    [JsonPropertyName("error_code")] public string ErrorCode { get; set; }
    [JsonPropertyName("error_message")] public string ErrorMessage { get; set; }
}