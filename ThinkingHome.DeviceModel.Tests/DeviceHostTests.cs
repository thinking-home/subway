using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

public class DeviceHostTests
{
    [Fact]
    public async Task Concurrent_first_queries_hit_the_driver_once() // single-flight
    {
        var host = new DeviceHost();
        var device = new StubDevice("lamp");
        host.Register(device);

        var results = await Task.WhenAll(Enumerable.Range(0, 8).Select(_ => host.QueryAsync("lamp")));

        Assert.Equal(8, results.Length);
        Assert.Equal(1, device.QueryCount);
    }

    [Fact]
    public async Task Report_updates_cache_without_hitting_driver_again()
    {
        var host = new DeviceHost();
        var device = new StubDevice("lamp");
        host.Register(device);

        _ = await host.QueryAsync("lamp"); // прайминг (1 обращение к драйверу)
        await host.ExecuteAsync("lamp", new OnOffCommand { Instance = "on", Value = true }); // → Report обновляет кэш

        var snapshot = await host.QueryAsync("lamp"); // из кэша
        var state = Assert.IsType<OnOffState>(Assert.Single(snapshot.Values));
        Assert.True(state.Value);
        Assert.Equal(1, device.QueryCount);
    }

    [Fact]
    public async Task Unknown_device_throws()
    {
        var host = new DeviceHost();
        await Assert.ThrowsAsync<KeyNotFoundException>(() => host.QueryAsync("nope"));
    }

    private sealed class StubDevice(string id) : IDevice
    {
        private bool isOn;
        public int QueryCount;

        public string Id => id;
        public event Action<StateChange>? Changed;

        public DeviceDescriptor Describe() => new()
        {
            Id = id,
            Title = id,
            Endpoints = [new Endpoint
            {
                Id = 0,
                Type = DeviceType.OnOffLight,
                Capabilities = [new OnOffCapability { Instance = "on" }],
            }],
        };

        public async Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        {
            Interlocked.Increment(ref QueryCount);
            await Task.Delay(30, ct); // окно для проверки single-flight
            return new DeviceSnapshot { DeviceId = id, Values = [new OnOffState { Instance = "on", Value = isOn }] };
        }

        public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
        {
            if (command is OnOffCommand cmd)
            {
                isOn = cmd.Value;
                Changed?.Invoke(new StateChange
                {
                    DeviceId = id,
                    Value = new OnOffState { Instance = "on", Value = isOn },
                });
                return Task.FromResult(CommandOutcome.Done);
            }

            return Task.FromResult(CommandOutcome.Unsupported);
        }
    }
}
