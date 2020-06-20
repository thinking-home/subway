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
}