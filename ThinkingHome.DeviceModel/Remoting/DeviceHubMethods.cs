namespace ThinkingHome.DeviceModel.Remoting;

/// <summary>
/// Имена методов SignalR-контракта между прокси и домашним коннектором. Общие для обеих сторон
/// (ProxyServer вызывает, ProxyClient обрабатывает) — чтобы имена не разъезжались.
/// </summary>
public static class DeviceHubMethods
{
    // прокси → дом (server → client, с результатом)
    /// <summary>Прокси → дом: запрос дескрипторов всех устройств.</summary>
    public const string GetDevices = "GetDevices";
    /// <summary>Прокси → дом: опрос состояния устройства.</summary>
    public const string Query = "Query";
    /// <summary>Прокси → дом: исполнение команды над устройством.</summary>
    public const string Execute = "Execute";
    /// <summary>Прокси → дом: сгенерировать и доставить пользователю OTP привязки.</summary>
    public const string GenerateLinkingOtp = "GenerateLinkingOtp";
    /// <summary>Прокси → дом: проверить OTP привязки.</summary>
    public const string ValidateLinkingOtp = "ValidateLinkingOtp";

    // дом → прокси (client → server)
    /// <summary>Дом → прокси: пуш изменения состояния.</summary>
    public const string Report = "Report";
}
