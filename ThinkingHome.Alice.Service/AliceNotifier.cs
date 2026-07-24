using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ThinkingHome.Alice.Mapping;
using ThinkingHome.Alice.Model.Callback;
using ThinkingHome.DeviceModel.Remoting.ProxyServer;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Alice.Service
{
    /// <summary>
    /// Пуш изменений состояния в Яндекс (Notification API): подписывается на Report'ы всех
    /// подключённых хостов и на каждое изменение шлёт callback/state с user_id = hostId.
    /// Без настроенных Alice:SkillId / Alice:CallbackToken работает вхолостую — логирует payload
    /// (проверка пайплайна без секретов). Батчинга нет: одно изменение — один POST.
    /// </summary>
    public sealed class AliceNotifier(
        IRemoteHostRegistry registry,
        IHttpClientFactory httpFactory,
        IConfiguration configuration,
        ILogger<AliceNotifier> logger) : IHostedService
    {
        private readonly ConcurrentDictionary<string, byte> subscribed = new();
        private string SkillId => configuration["Alice:SkillId"];
        private string CallbackToken => configuration["Alice:CallbackToken"];

        public Task StartAsync(CancellationToken cancellationToken)
        {
            registry.HostConnected += Subscribe;
            foreach (var hostId in registry.ConnectedHosts)
            {
                Subscribe(hostId);
            }

            logger.LogInformation("Пуш состояний в Алису: {Mode}",
                string.IsNullOrEmpty(SkillId) || string.IsNullOrEmpty(CallbackToken)
                    ? "выключен (Alice:SkillId/Alice:CallbackToken не заданы), payload идёт в лог"
                    : $"включен (навык {SkillId})");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private void Subscribe(string hostId)
        {
            // RemoteHost переживает реконнекты, подписка нужна одна на hostId за жизнь процесса
            if (!subscribed.TryAdd(hostId, 0)) return;

            if (!registry.TryGet(hostId, out var host))
            {
                subscribed.TryRemove(hostId, out _); // хост успел отвалиться — подпишемся на следующем коннекте
                return;
            }

            host.Changed += change => _ = NotifyAsync(hostId, change);
        }

        private async Task NotifyAsync(string hostId, StateChange change)
        {
            try
            {
                var request = new CallbackStateRequest
                {
                    Ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Payload = new CallbackStatePayload
                    {
                        UserId = hostId,
                        Devices = [AliceMapper.ToDeviceState(change)],
                    },
                };

                if (string.IsNullOrEmpty(SkillId) || string.IsNullOrEmpty(CallbackToken))
                {
                    logger.LogInformation("Пуш в Алису (выключен, только лог): {Payload}",
                        JsonSerializer.Serialize(request));
                    return;
                }

                using var http = httpFactory.CreateClient();
                using var message = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"https://dialogs.yandex.net/api/v1/skills/{SkillId}/callback/state");
                message.Headers.TryAddWithoutValidation("Authorization", $"OAuth {CallbackToken}");
                message.Content = JsonContent.Create(request);

                using var response = await http.SendAsync(message);
                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation("Пуш принят Яндексом ({Status}): {DeviceId}",
                        (int)response.StatusCode, change.DeviceId);
                }
                else
                {
                    logger.LogWarning("Яндекс не принял пуш состояния: {Status} {Body}",
                        (int)response.StatusCode, await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка пуша состояния в Алису (host {HostId})", hostId);
            }
        }
    }
}
