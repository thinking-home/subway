using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Event;

// Значения событий из фиксированного словаря Алисы. Пока — наборы для motion и open.
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertyEventValue
{
    [JsonStringEnumMemberName("detected")] Detected,
    [JsonStringEnumMemberName("not_detected")] NotDetected,
    [JsonStringEnumMemberName("opened")] Opened,
    [JsonStringEnumMemberName("closed")] Closed,
}
