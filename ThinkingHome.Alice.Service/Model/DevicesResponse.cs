using ThinkingHome.Alice.Service.Model.Devices;
using ThinkingHome.Alice.Service.Model.Devices.Capability;

namespace ThinkingHome.Alice.Service.Model
{
    public class DevicesResponse
    {
        public string request_id { get; set; }

        public DevicesPayload payload { get; set; }
    }

    public class DevicesQueryPayload
    {
        public DeviceState[] devices { get; set; }
    }

    public class DeviceState
    {
        public string id { get; set; }
        public CapabilityState[] capabilities { get; set; }
    }

    public class CapabilityState
    {
        public CapabilityType type { get; set; }
        public CapabilityStateModelBase state { get; set; }
    }
}