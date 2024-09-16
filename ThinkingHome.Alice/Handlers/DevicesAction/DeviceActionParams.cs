using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities;

namespace ThinkingHome.Alice.Handlers.DevicesAction
{
    public class DeviceActionParams
    {
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("custom_data")] public object CustomData { get; set; }
        [JsonPropertyName("capabilities")] public CapabilityActionParamsBase[] Capabilities { get; set; }
    }
}