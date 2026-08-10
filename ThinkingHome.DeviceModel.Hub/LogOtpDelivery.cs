using ThinkingHome.DeviceModel.Remoting.ProxyClient;

namespace ThinkingHome.Home;

/// <summary>
/// Доставка OTP в лог хоста: владелец читает код, залогинившись на машину (доступ к логам = право привязки).
/// Одна из возможных реализаций — потребитель может заменить на email и т.п.
/// </summary>
public sealed class LogOtpDelivery : IOtpDelivery
{
    public Task DeliverAsync(string otp, CancellationToken ct = default)
    {
        Console.WriteLine($"[ПРИВЯЗКА] Одноразовый код: {otp} (действует 2 минуты)");
        return Task.CompletedTask;
    }
}
