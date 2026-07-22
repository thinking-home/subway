using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThinkingHome.Alice.Handlers.DevicesAction;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.Alice.Service;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Remoting.ProxyServer;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

public class AliceControllerTests
{
    private static AliceController Controller(IDeviceHost? host) =>
        new(new FakeRegistry(host), new StaticHostIdResolver("home"))
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        };

    [Fact]
    public async Task Devices_online_maps_descriptors_with_composite_id()
    {
        var response = await Controller(new FakeHost()).Devices("req-1");

        Assert.Equal("home", response.Payload.UserId);
        var device = Assert.Single(response.Payload.Devices);
        Assert.Equal("lamp#0", device.Id);
    }

    [Fact]
    public async Task Devices_offline_returns_empty()
    {
        var response = await Controller(host: null).Devices("req-1");
        Assert.Empty(response.Payload.Devices);
    }

    [Fact]
    public async Task Action_parses_composite_id_and_executes_on_host()
    {
        var host = new FakeHost();
        var request = new DevicesActionRequest
        {
            Payload = new DevicesActionRequestPayload
            {
                Devices =
                [
                    new DeviceActionParams
                    {
                        Id = "lamp#0",
                        Capabilities =
                        [
                            new CapabilityActionParamsOnOff
                            {
                                State = new CapabilityStateOnOffData
                                {
                                    Instance = CapabilityStateOnOffInstance.On,
                                    Value = true,
                                },
                            },
                        ],
                    },
                ],
            },
        };

        var response = await Controller(host).DevicesAction("req-1", request);

        var executed = Assert.Single(host.Executed);
        Assert.Equal("lamp", executed.DeviceId); // распарсен из "lamp#0"
        var command = Assert.IsType<OnOffCommand>(executed.Command);
        Assert.Equal(0, command.EndpointId);
        Assert.True(command.Value);
        Assert.Single(response.Payload.Devices);
    }

#pragma warning disable CS0067 // события интерфейсов в тестовых фейках не используются
    private sealed class FakeRegistry(IDeviceHost? backing) : IRemoteHostRegistry
    {
        public bool TryGet(string hostId, [NotNullWhen(true)] out IDeviceHost? host)
        {
            host = backing;
            return backing is not null;
        }

        public IReadOnlyCollection<string> ConnectedHosts => backing is null ? [] : ["home"];
        public event Action<string>? HostConnected;
        public event Action<string>? HostDisconnected;
    }

    private sealed class FakeHost : IDeviceHost
    {
        public List<(string DeviceId, DeviceCommand Command)> Executed { get; } = [];

        public event Action<StateChange>? Changed;

        public Task<IReadOnlyCollection<DeviceDescriptor>> GetDevicesAsync(CancellationToken ct = default)
            => Task.FromResult<IReadOnlyCollection<DeviceDescriptor>>([Lamp()]);

        public Task<DeviceDescriptor?> GetDeviceAsync(string deviceId, CancellationToken ct = default)
            => Task.FromResult<DeviceDescriptor?>(deviceId == "lamp" ? Lamp() : null);

        public Task<DeviceSnapshot> QueryAsync(string deviceId, CancellationToken ct = default)
            => Task.FromResult(new DeviceSnapshot
            {
                DeviceId = deviceId,
                Values = [new OnOffState { Instance = "on_off", Value = true }],
            });

        public Task<CommandOutcome> ExecuteAsync(string deviceId, DeviceCommand command, CancellationToken ct = default)
        {
            Executed.Add((deviceId, command));
            return Task.FromResult(CommandOutcome.Done);
        }

        private static DeviceDescriptor Lamp() => new()
        {
            Id = "lamp",
            Title = "Лампа",
            Endpoints = [new Endpoint
            {
                Id = 0,
                Type = DeviceType.OnOffLight,
                Capabilities = [new OnOffCapability { Instance = "on_off" }],
            }],
        };
    }
}
