using ThinkingHome.Alice.Model.Capabilities;

namespace ThinkingHome.Alice.Model;

public class Device
{
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string room { get; set; }
    public DeviceType type { get; set; }
    public object custom_data { get; set; }
    public CapabilityInfoBase[] capabilities { get; set; }
    public object[] properties { get; set; }

    public DeviceInfo device_info { get; set; }
}