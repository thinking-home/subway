using ThinkingHome.Alice.Mapping;
using ThinkingHome.Alice.Model.ActionResult;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;
using AliceDeviceType = ThinkingHome.Alice.Model.DeviceType;

namespace ThinkingHome.DeviceModel.Tests;

public class AliceMapperTests
{
    [Fact]
    public void ToDevices_single_endpoint_uses_composite_id()
    {
        var descriptor = new DeviceDescriptor
        {
            Id = "lamp-1",
            Title = "Лампа",
            Room = "Кухня",
            Endpoints = [new Endpoint
            {
                Id = 0,
                Type = DeviceType.OnOffLight,
                Capabilities = [new OnOffCapability { Instance = "on" }],
            }],
        };

        var device = Assert.Single(AliceMapper.ToDevices(descriptor));

        Assert.Equal("lamp-1#0", device.Id);
        Assert.Equal("Лампа", device.Name);
        Assert.Equal("Кухня", device.Room);
        Assert.Equal(AliceDeviceType.Light, device.Type);
        Assert.IsType<CapabilityInfoOnOff>(Assert.Single(device.Capabilities));
    }

    [Fact]
    public void ToDevices_splits_endpoints_into_separate_devices()
    {
        var descriptor = new DeviceDescriptor
        {
            Id = "switch",
            Title = "Выключатель",
            Endpoints =
            [
                new Endpoint { Id = 0, Type = DeviceType.OnOffLight, Capabilities = [new OnOffCapability { Instance = "on" }] },
                new Endpoint { Id = 1, Type = DeviceType.OnOffLight, Capabilities = [new OnOffCapability { Instance = "on" }] },
            ],
        };

        var ids = AliceMapper.ToDevices(descriptor).Select(d => d.Id).ToArray();

        Assert.Equal(new[] { "switch#0", "switch#1" }, ids);
    }

    [Fact]
    public void ToDeviceState_filters_values_by_endpoint()
    {
        var snapshot = new DeviceSnapshot
        {
            DeviceId = "switch",
            Values =
            [
                new OnOffState { EndpointId = 0, Instance = "on", Value = false },
                new OnOffState { EndpointId = 1, Instance = "on", Value = true },
            ],
        };

        var state = AliceMapper.ToDeviceState(new AliceDeviceId("switch", 1), snapshot);

        Assert.Equal("switch#1", state.Id);
        var cap = Assert.IsType<CapabilityStateOnOff>(Assert.Single(state.Capabilities));
        Assert.True(cap.State.Value); // значение именно endpoint'а 1
    }

    [Fact]
    public void ToCommand_sets_endpoint()
    {
        var action = new CapabilityActionParamsOnOff
        {
            State = new CapabilityStateOnOffData { Instance = CapabilityStateOnOffInstance.On, Value = true },
        };

        var command = Assert.IsType<OnOffCommand>(AliceMapper.ToCommand(action, endpointId: 2));

        Assert.Equal(2, command.EndpointId);
        Assert.Equal("on", command.Instance);
        Assert.True(command.Value);
    }

    [Fact]
    public void ToActionResult_maps_done_and_error()
    {
        Assert.Equal(ActionResultStatus.DONE, AliceMapper.ToActionResult(CommandOutcome.Done).Status);

        var error = AliceMapper.ToActionResult(CommandOutcome.Error(CommandErrorCode.DeviceUnreachable, "нет связи"));
        Assert.Equal(ActionResultStatus.ERROR, error.Status);
        Assert.Equal(ActionResultErrorCode.DEVICE_UNREACHABLE, error.ErrorCode);
    }

    [Fact]
    public void AliceDeviceId_roundtrips_and_parses_defensively()
    {
        Assert.Equal("lamp-1#0", new AliceDeviceId("lamp-1", 0).ToAlice());

        var id = AliceDeviceId.Parse("lamp-1#2");
        Assert.Equal("lamp-1", id.DeviceId);
        Assert.Equal(2, id.EndpointId);

        Assert.Equal(new AliceDeviceId("legacy", 0), AliceDeviceId.Parse("legacy"));
    }

    [Theory]
    [InlineData(DeviceType.OnOffLight, AliceDeviceType.Light)]
    [InlineData(DeviceType.OnOffSocket, AliceDeviceType.Socket)]
    [InlineData(DeviceType.OnOffSwitch, AliceDeviceType.Switch)]
    public void ToDevices_maps_onoff_device_types(DeviceType type, AliceDeviceType expected)
    {
        var descriptor = new DeviceDescriptor
        {
            Id = "d",
            Title = "T",
            Endpoints = [new Endpoint { Id = 0, Type = type, Capabilities = [new OnOffCapability { Instance = "on" }] }],
        };

        var device = Assert.Single(AliceMapper.ToDevices(descriptor));
        Assert.Equal(expected, device.Type);
    }
}
