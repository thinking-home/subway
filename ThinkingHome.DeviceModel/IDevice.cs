using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel;

/// <summary>
/// Контракт драйвера (провайдер-грань). Реализуется классом, умеющим общаться с конкретным
/// физическим устройством. Регистрируется в <see cref="IDeviceRegistry"/>.
/// </summary>
public interface IDevice
{
    /// <summary>Стабильный идентификатор устройства.</summary>
    string Id { get; }

    /// <summary>Описание устройства (discovery).</summary>
    DeviceDescriptor Describe();

    /// <summary>Опросить текущее состояние (query).</summary>
    Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default);

    /// <summary>Исполнить команду (execute). Незнакомую команду вернуть как <see cref="CommandOutcome.Unsupported"/>.</summary>
    Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default);

    /// <summary>Изменение состояния (report/push).</summary>
    event Action<StateChange> Changed;
}
