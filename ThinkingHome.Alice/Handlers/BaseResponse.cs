using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Handlers;

/// <summary>Базовый класс ответов на запросы платформы умного дома Яндекса: несёт общее поле request_id.</summary>
public abstract class BaseResponse
{
    /// <summary>Идентификатор запроса; в ответе возвращается тот же, что пришёл в запросе Алисы.</summary>
    [JsonPropertyName("request_id")] public string RequestId { get; set; }
}