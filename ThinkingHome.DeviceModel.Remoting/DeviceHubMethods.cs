namespace ThinkingHome.DeviceModel.Remoting;

/// <summary>
/// Имена методов SignalR-контракта между прокси и домашним коннектором. Общие для обеих сторон
/// (ProxyServer вызывает, ProxyClient обрабатывает) — чтобы имена не разъезжались.
/// </summary>
public static class DeviceHubMethods
{
    // прокси → дом (server → client, с результатом)
    public const string GetDevices = "GetDevices";
    public const string Query = "Query";
    public const string Execute = "Execute";
    public const string GenerateLinkingOtp = "GenerateLinkingOtp";
    public const string ValidateLinkingOtp = "ValidateLinkingOtp";

    // дом → прокси (client → server)
    public const string Report = "Report";
}
