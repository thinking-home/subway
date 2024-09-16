using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Capabilities;

namespace ThinkingHome.Alice.Model;

public class Device
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("room")]
    public string Room { get; set; }
    
    [JsonPropertyName("type")]
    public DeviceType Type { get; set; }
    
    [JsonPropertyName("custom_data")]
    public object CustomData { get; set; }
    
    [JsonPropertyName("capabilities")]
    public CapabilityInfoBase[] Capabilities { get; set; }
    
    [JsonPropertyName("properties")]
    public object[] Properties { get; set; }

    [JsonPropertyName("device_info")]
    public DeviceInfo DeviceInfo { get; set; }
}