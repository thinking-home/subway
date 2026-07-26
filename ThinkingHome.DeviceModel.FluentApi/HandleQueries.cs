using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi;

/// <summary>
/// Общие запросы хендлов к ядру. Чтение — это Query устройства плюс выбор своего значения из
/// снапшота (отдельного канала чтения в ядре нет); discovery — выбор своего фрагмента дескриптора.
/// Отсутствие — null, транспорт и неизвестное устройство в Query — исключения ядра.
/// </summary>
internal static class HandleQueries
{
    /// <summary>Значение (endpoint, instance, тип) из снапшота или null, если его там нет.</summary>
    public static async Task<TState?> GetStateAsync<TState>(
        this IDeviceHost host, string deviceId, int endpointId, string instance, CancellationToken ct)
        where TState : StateValue
    {
        var snapshot = await host.QueryAsync(deviceId, ct);
        return snapshot.Values.OfType<TState>()
            .FirstOrDefault(v => v.EndpointId == endpointId && v.Instance == instance);
    }

    /// <summary>Endpoint дескриптора или null, если endpoint'а (или устройства) нет.</summary>
    public static async Task<Endpoint?> GetEndpointAsync(
        this IDeviceHost host, string deviceId, int endpointId, CancellationToken ct)
    {
        var descriptor = await host.GetDeviceAsync(deviceId, ct);
        return descriptor?.Endpoints.FirstOrDefault(e => e.Id == endpointId);
    }

    /// <summary>Описание способности (endpoint, instance, тип) или null, если её нет.</summary>
    public static async Task<TCapability?> GetCapabilityAsync<TCapability>(
        this IDeviceHost host, string deviceId, int endpointId, string instance, CancellationToken ct)
        where TCapability : Capability
        => (await host.GetEndpointAsync(deviceId, endpointId, ct))?
            .Capabilities.OfType<TCapability>().FirstOrDefault(c => c.Instance == instance);

    /// <summary>Описание свойства (endpoint, instance, тип) или null, если его нет.</summary>
    public static async Task<TProperty?> GetPropertyAsync<TProperty>(
        this IDeviceHost host, string deviceId, int endpointId, string instance, CancellationToken ct)
        where TProperty : Property
        => (await host.GetEndpointAsync(deviceId, endpointId, ct))?
            .Properties.OfType<TProperty>().FirstOrDefault(p => p.Instance == instance);
}
