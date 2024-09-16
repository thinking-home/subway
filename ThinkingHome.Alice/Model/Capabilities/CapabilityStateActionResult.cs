using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities;

public class CapabilityStateActionResult<T>
{
    [JsonPropertyName("instance")] public T Instance { get; set; }
    [JsonPropertyName("action_result")] public ActionResult.ActionResult ActionResult { get; set; }
}