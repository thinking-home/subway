using ThinkingHome.DeviceModel.Remoting.ProxyClient;

namespace ThinkingHome.DeviceModel.Hub;

/// <summary>
/// Встроенный коннектор облачного прокси: оборачивает <see cref="Connector"/> в фоновый сервис хоста.
/// Подключается с повторами, не блокируя старт приложения; соединение живёт до остановки хаба
/// (реконнекты после обрыва — внутри Connector).
/// </summary>
internal sealed class ProxyConnectorService(
    IDeviceHost host,
    IOtpDelivery otpDelivery,
    IConfiguration config,
    ILogger<ProxyConnectorService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var url = config["Proxy:Url"]!;
        var token = config["Proxy:HostToken"];

        if (string.IsNullOrEmpty(token))
        {
            logger.LogWarning("Proxy:HostToken не задан — прокси с авторизацией отклонит подключение");
        }

        await using var connector = new Connector(host, otpDelivery, url, () => Task.FromResult(token));

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await connector.StartAsync(stoppingToken);
                logger.LogInformation("Подключено к прокси {Url}", url);
                break;
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                logger.LogWarning("Не удалось подключиться к прокси: {Message}; повтор через 5 с", ex.Message);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        // соединение держится до остановки приложения
        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            // штатная остановка
        }
    }
}
