using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.Callback;

/// <summary>
/// Запрос Notification API Яндекса: уведомление об изменении состояния устройств
/// (POST https://dialogs.yandex.net/api/v1/skills/{skill_id}/callback/state).
/// </summary>
public class CallbackStateRequest
{
    /// <summary>Unix-время события, секунды.</summary>
    [JsonPropertyName("ts")] public long Ts { get; set; }

    [JsonPropertyName("payload")] public CallbackStatePayload Payload { get; set; }
}

public class CallbackStatePayload
{
    /// <summary>Пользователь провайдера — тот же id, что отдаётся в discovery (у нас hostId).</summary>
    [JsonPropertyName("user_id")] public string UserId { get; set; }

    [JsonPropertyName("devices")] public DeviceState[] Devices { get; set; }
}
