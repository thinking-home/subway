namespace ThinkingHome.DeviceModel;

/// <summary>
/// Провайдер-грань хоста: драйверы регистрируют экземпляры устройств.
/// </summary>
public interface IDeviceRegistry
{
    /// <summary>Зарегистрировать устройство. Dispose() возвращаемого объекта снимает регистрацию.</summary>
    IDisposable Register(IDevice device);
}
