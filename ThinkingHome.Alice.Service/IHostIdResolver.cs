using Microsoft.AspNetCore.Http;
using ThinkingHome.DeviceModel.Remoting.ProxyServer;

namespace ThinkingHome.Alice.Service
{
    /// <summary>
    /// Определяет, к какому домашнему хосту (hostId) относится запрос Алисы.
    /// </summary>
    public interface IHostIdResolver
    {
        string Resolve(HttpContext context);
    }

    /// <summary>hostId из claim аутентифицированного access token Алисы.</summary>
    public sealed class ClaimHostIdResolver : IHostIdResolver
    {
        public string Resolve(HttpContext context) => context.User.FindFirst(HostToken.HostIdClaim)?.Value ?? "";
    }

    /// <summary>Фиксированный hostId (для тестов/локального запуска без аутентификации).</summary>
    public sealed class StaticHostIdResolver(string hostId) : IHostIdResolver
    {
        public string Resolve(HttpContext context) => hostId;
    }
}
