using ThinkingHome.Alice.Model;

namespace ThinkingHome.Alice.Handlers.Devices
{
    public class DevicesPayload
    {
        public string user_id { get; set; }

        public Device[] devices { get; set; }
    }
}