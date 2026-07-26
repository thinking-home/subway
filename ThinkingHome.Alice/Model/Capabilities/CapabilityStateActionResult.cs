using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Capabilities;

/// <summary>Результат операции в ответе на action: инстанс способности и итог выполнения команды.</summary>
public class CapabilityStateActionResult<T>
{
    /// <summary>Инстанс способности, к которому применялась команда.</summary>
    [JsonPropertyName("instance")] public T Instance { get; set; }
    /// <summary>Результат выполнения команды.</summary>
    [JsonPropertyName("action_result")] public ActionResult.ActionResult ActionResult { get; set; }
}