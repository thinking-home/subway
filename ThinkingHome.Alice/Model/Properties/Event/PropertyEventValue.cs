using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Properties.Event;

/// <summary>Значения событий из фиксированного словаря Алисы. Пока — наборы для motion, open и water_leak.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertyEventValue
{
    /// <summary>Движение обнаружено.</summary>
    [JsonStringEnumMemberName("detected")] Detected,
    /// <summary>Движение не обнаружено.</summary>
    [JsonStringEnumMemberName("not_detected")] NotDetected,
    /// <summary>Открыто.</summary>
    [JsonStringEnumMemberName("opened")] Opened,
    /// <summary>Закрыто.</summary>
    [JsonStringEnumMemberName("closed")] Closed,
    /// <summary>Сухо, протечки нет.</summary>
    [JsonStringEnumMemberName("dry")] Dry,
    /// <summary>Обнаружена протечка.</summary>
    [JsonStringEnumMemberName("leak")] Leak,
}
