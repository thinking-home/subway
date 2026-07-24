using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
    /// подключённых хостов, копит изменения в окне <see cref="FlushWindow"/> и шлёт их одним
    /// callback/state на хост (user_id = hostId) — Яндекс просит объединять изменения и слать
    /// не чаще ~раза в секунду. Дедуп внутри окна — в чистом маппере (последнее значение слота
    /// побеждает). Без настроенных Alice:SkillId / Alice:CallbackToken работает вхолостую —
    /// логирует payload (проверка пайплайна без секретов).
    /// </summary>
    public sealed class AliceNotifier(
        IRemoteHostRegistry registry,
        IHttpClientFactory httpFactory,
        IConfiguration configuration,
        ILogger<AliceNotifier> logger) : IHostedService
    {
        // окно накопления: максимум одна отправка на хост в секунду, худшая задержка пуша — то же окно
        private static readonly TimeSpan FlushWindow = TimeSpan.FromSeconds(1);

        private readonly ConcurrentDictionary<string, byte> subscribed = new();
        private readonly Lock gate = new();
        private readonly List<(string HostId, StateChange Change)> buffer = [];
        private bool flushScheduled;

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

            host.Changed += change => Enqueue(hostId, change);
        }

        private void Enqueue(string hostId, StateChange change)
        {
            lock (gate)
            {
                buffer.Add((hostId, change));
                if (flushScheduled) return;
                flushScheduled = true;
            }

            _ = FlushAfterWindowAsync();
        }

        private async Task FlushAfterWindowAsync()
        {
            try
            {
                await Task.Delay(FlushWindow);

                List<(string HostId, StateChange Change)> drained;
                lock (gate)
                {
                    drained = [.. buffer];
                    buffer.Clear();
                    flushScheduled = false;
                }

                var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                foreach (var host in drained.GroupBy(x => x.HostId))
                {
                    await SendAsync(AliceMapper.ToCallbackState(host.Key, host.Select(x => x.Change).ToArray(), ts));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка пуша состояний в Алису");
            }
        }

        private async Task SendAsync(CallbackStateRequest request)
        {
            var deviceIds = string.Join(", ", request.Payload.Devices.Select(d => d.Id));

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
                logger.LogInformation("Пуш принят Яндексом ({Status}): {DeviceIds}",
                    (int)response.StatusCode, deviceIds);
            }
            else
            {
                logger.LogWarning("Яндекс не принял пуш состояния ({DeviceIds}): {Status} {Body}",
                    deviceIds, (int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
    }
}
