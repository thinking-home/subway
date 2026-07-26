using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.FluentApi;

/// <summary>Подписка на поток изменений хоста; Dispose — отписка (идемпотентно).</summary>
internal sealed class ChangedSubscription : IDisposable
{
    private readonly Action<StateChange> handler;
    private IDeviceHost? host;

    public ChangedSubscription(IDeviceHost host, Action<StateChange> handler)
    {
        this.host = host;
        this.handler = handler;
        host.Changed += handler;
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref host, null) is { } h) h.Changed -= handler;
    }
}
