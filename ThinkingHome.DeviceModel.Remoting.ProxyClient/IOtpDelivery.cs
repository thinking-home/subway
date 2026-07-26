namespace ThinkingHome.DeviceModel.Remoting.ProxyClient;

/// <summary>
/// Доставка одноразового пароля привязки пользователю (лог хоста, email, …). Реализацию предоставляет
/// потребитель библиотеки (домашнее приложение); в самой библиотеке её нет.
/// </summary>
public interface IOtpDelivery
{
    /// <summary>Доставить одноразовый код пользователю (лог, email, мессенджер — на усмотрение реализации).</summary>
    Task DeliverAsync(string otp, CancellationToken ct = default);
}
