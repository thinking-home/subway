using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.ActionResult
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActionResultStatus
    {
        [JsonStringEnumMemberName("DONE")] DONE,
        [JsonStringEnumMemberName("ERROR")] ERROR,
    }
}