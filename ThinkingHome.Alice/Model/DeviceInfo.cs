using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model;

public class DeviceInfo
{
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; }
    
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    [JsonPropertyName("hw_version")]
    public string HardwareVersion { get; set; }
    
    [JsonPropertyName("sw_version")]
    public string SoftwareVersion { get; set; }
}