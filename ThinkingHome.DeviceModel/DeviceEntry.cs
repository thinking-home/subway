using System.Collections.Concurrent;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel;

/// <summary>
/// Запись об одном зарегистрированном устройстве. Инкапсулирует host-level кэш последнего состояния
/// и подписку на изменения устройства. О самом хосте не знает — общается с ним только через два
/// колбэка, полученных в конструкторе: <paramref name="onChanged"/> (фан-аут изменений наверх) и
/// <paramref name="onRemoved"/> (самоудаление из хоста при <see cref="Dispose"/>).
/// </summary>
internal sealed class DeviceEntry(
    IDevice device,
    Action<StateChange> onChanged,
    Action<DeviceEntry> onRemoved) : IDisposable
{
    private readonly ConcurrentDictionary<(int EndpointId, string Instance), StateValue> cache = new();

    // single-flight на заполнение кэша: первый запрос идёт в драйвер, остальные ждут его
    private readonly SemaphoreSlim primeGate = new(1, 1);
    private volatile bool primed;

    public string Id => device.Id;

    /// <summary>Подписаться на изменения устройства. Вызывается хостом после успешной регистрации.</summary>
    public void Attach() => device.Changed += HandleChanged;

    public DeviceDescriptor Describe() => device.Describe();

    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct)
        => device.ExecuteAsync(command, ct);

    /// <summary>
    /// Отвечает из кэша; если кэш ещё не заполнен — единожды идёт в драйвер (фолбэк). Параллельные
    /// первые запросы коалесцируются: драйвер спрашивается один раз, остальные ждут и берут из кэша.
    /// </summary>
    public async Task<DeviceSnapshot> QueryAsync(CancellationToken ct)
    {
        if (!primed)
        {
            await primeGate.WaitAsync(ct);
            try
            {
                if (!primed)
                {
                    var snapshot = await device.QueryAsync(ct);

                    foreach (var value in snapshot.Values)
                    {
                        cache[(value.EndpointId, value.Instance)] = value;
                    }

                    primed = true;
                }
            }
            finally
            {
                primeGate.Release();
            }
        }

        return Snapshot();
    }

    private DeviceSnapshot Snapshot() => new()
    {
        DeviceId = device.Id,
        Values = cache.Values.ToArray(),
    };

    private void HandleChanged(StateChange change)
    {
        cache[(change.Value.EndpointId, change.Value.Instance)] = change.Value;
        onChanged(change);
    }

    public void Dispose()
    {
        device.Changed -= HandleChanged;
        onRemoved(this);
        primeGate.Dispose();
    }
}
