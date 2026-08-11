namespace ThinkingHome.DeviceModel;

/// <summary>
/// Плагин хаба — источник устройств. Создаёт устройства по собственной конфигурации (читает её
/// сам, обычно через IConfiguration из DI) и регистрирует их в реестре. Плагин с собственным
/// жизненным циклом (подключение к брокеру, динамический список устройств) дополнительно
/// реализует IHostedService — хаб поднимет его как фоновый сервис.
/// </summary>
public interface IDevicePlugin
{
    /// <summary>Создать устройства по своей конфигурации и зарегистрировать их в реестре.</summary>
    void RegisterDevices(IDeviceRegistry registry);
}
