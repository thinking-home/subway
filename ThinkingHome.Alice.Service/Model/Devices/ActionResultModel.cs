namespace ThinkingHome.Alice.Service.Model.Devices
{
    public class ActionResultModel
    {
        public ActionResultStatus status { get; set; }

        public ActionResultErrorCode error_code { get; set; }

        public string error_message { get; set; }
    }

    public class DeviceActionResult
    {
        public string id { get; set; } // id устройства
        public ActionResultModel action_result { get; set; } // общий код ответа
        public CapabilityActionResult[] capabilities { get; set; }
    }

    public class CapabilityActionResult
    {
        // ответ с результатом операции над конкретным умением

        public CapabilityType type { get; set; }
        public CapabilityStateActionResult state { get; set; }
    }

    public class CapabilityStateActionResult
    {
        public string instance { get; set; }
        public ActionResultModel action_result { get; set; }
    }
}