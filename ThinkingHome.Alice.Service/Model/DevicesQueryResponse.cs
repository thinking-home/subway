namespace ThinkingHome.Alice.Service.Model
{
    public class DevicesQueryResponse
    {
        public string request_id { get; set; }

        public DevicesQueryPayload payload { get; set; }
    }

    public class DevicesQueryRequest
    {
        public DeviceReference[] devices { get; set; }
    }

    public class DeviceReference
    {
        public string id { get; set; }
        public object custom_data { get; set; }
    }

    public class DevicesQueryPayload
    {
        public DeviceState[] devices { get; set; }
    }

    public class DeviceState
    {
        public string id { get; set; }
        // public CapabilityState[] capabilities { get; set; }
    }

    // public class CapabilityState
    // {
    //     public CapabilityType type { get; set; }
    //     public CapabilityStateModelBase state { get; set; }
    //
    //     public CapabilityActionResult GetActionResult(ActionResultModel result)
    //     {
    //         return new CapabilityActionResult
    //         {
    //             type = type,
    //             state = new CapabilityStateActionResult
    //             {
    //                 instance = state.instance,
    //                 action_result = result
    //             }
    //         };
    //     }
    // }
}