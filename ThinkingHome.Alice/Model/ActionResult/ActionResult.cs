using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.ActionResult
{
    public class ActionResult
    {
        [JsonPropertyName("status")]
        public ActionResultStatus Status { get; set; }

        [JsonPropertyName("error_code")]
        public ActionResultErrorCode? ErrorCode { get; set; }

        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; }

        public static ActionResult Ok => new() { Status = ActionResultStatus.DONE };

        public static ActionResult InvalidValue(string message = null)
        {
            return new ActionResult
            {
                Status = ActionResultStatus.ERROR,
                ErrorCode = ActionResultErrorCode.INVALID_VALUE,
                ErrorMessage = message
            };
        }
    }
}