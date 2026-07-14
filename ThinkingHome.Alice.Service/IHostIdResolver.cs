using Microsoft.AspNetCore.Http;

namespace ThinkingHome.Alice.Service
{
    /// <summary>
    /// Определяет, к какому домашнему хосту (hostId) относится запрос Алисы. Здесь будет реальный
    /// маппинг пользователя OAuth → hostId; пока статический (одно домохозяйство).
    /// </summary>
    public interface IHostIdResolver
    {
        string Resolve(HttpContext context);
    }

    public sealed class StaticHostIdResolver(string hostId) : IHostIdResolver
    {
        public string Resolve(HttpContext context) => hostId;
    }
}
