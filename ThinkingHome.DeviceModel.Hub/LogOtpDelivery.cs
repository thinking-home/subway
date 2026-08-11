using ThinkingHome.DeviceModel.Remoting.ProxyClient;

namespace ThinkingHome.DeviceModel.Hub;

/// <summary>Доставка кода привязки в журнал хаба — код вводят на странице привязки прокси.</summary>
internal sealed class LogOtpDelivery(ILogger<LogOtpDelivery> logger) : IOtpDelivery
{
    public Task DeliverAsync(string otp, CancellationToken ct = default)
    {
        logger.LogInformation("[ПРИВЯЗКА] Одноразовый код: {Otp} (действует 2 минуты)", otp);
        return Task.CompletedTask;
    }
}
