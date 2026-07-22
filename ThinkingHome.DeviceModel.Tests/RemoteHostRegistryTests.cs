using Microsoft.AspNetCore.SignalR;
using ThinkingHome.DeviceModel.Remoting.ProxyServer;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

public class RemoteHostRegistryTests
{
    private static StateChange Change(string deviceId) =>
        new() { DeviceId = deviceId, Value = new OnOffState { Instance = "on_off", Value = true } };

    [Fact]
    public void Report_is_routed_to_the_owning_host()
    {
        var registry = new RemoteHostRegistry(new StubHubContext());
        registry.Attach("home-A", "conn-A");
        registry.Attach("home-B", "conn-B");

        Assert.True(registry.TryGet("home-A", out var hostA));
        var received = new List<string>();
        hostA!.Changed += c => received.Add(c.DeviceId);

        registry.DispatchReport("conn-A", Change("lamp-A"));
        registry.DispatchReport("conn-B", Change("lamp-B"));

        Assert.Equal(new[] { "lamp-A" }, received);
    }

    [Fact]
    public void Reconnect_last_wins_and_late_detach_of_old_connection_keeps_host_online()
    {
        var registry = new RemoteHostRegistry(new StubHubContext());
        registry.Attach("home-A", "conn-1");
        registry.Attach("home-A", "conn-2"); // реконнект: новое соединение вытесняет старое
        registry.Detach("conn-1");           // поздний disconnect старого соединения

        Assert.True(registry.TryGet("home-A", out var host));

        var received = new List<string>();
        host!.Changed += c => received.Add(c.DeviceId);
        registry.DispatchReport("conn-2", Change("lamp"));
        Assert.Equal(new[] { "lamp" }, received);
    }

    [Fact]
    public void Full_detach_makes_host_offline()
    {
        var registry = new RemoteHostRegistry(new StubHubContext());
        registry.Attach("home-A", "conn-1");
        registry.Detach("conn-1");

        Assert.False(registry.TryGet("home-A", out _));
        Assert.Empty(registry.ConnectedHosts);
    }

    [Fact]
    public void TryGet_unknown_host_returns_false()
    {
        var registry = new RemoteHostRegistry(new StubHubContext());
        Assert.False(registry.TryGet("nope", out _));
    }

    // IHubContext<DeviceHub>, у которого Clients/Groups в этих тестах не нужны (команды не шлём)
    private sealed class StubHubContext : IHubContext<DeviceHub>
    {
        public IHubClients Clients => throw new NotSupportedException();
        public IGroupManager Groups => throw new NotSupportedException();
    }
}
