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
    private static DeviceDescriptor Lamp() => new()
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

    [Fact]
    public void ToDevice_maps_fields_and_onoff_capability()
    {
        var device = AliceMapper.ToDevice(Lamp());

        Assert.Equal("lamp-1", device.Id);
        Assert.Equal("Лампа", device.Name);
        Assert.Equal("Кухня", device.Room);
        Assert.Equal(AliceDeviceType.Light, device.Type);
        Assert.IsType<CapabilityInfoOnOff>(Assert.Single(device.Capabilities));
    }

    [Fact]
    public void ToDeviceState_maps_onoff_state()
    {
        var snapshot = new DeviceSnapshot
        {
            DeviceId = "lamp-1",
            Values = [new OnOffState { Instance = "on", Value = true }],
        };

        var state = AliceMapper.ToDeviceState(snapshot);

        Assert.Equal("lamp-1", state.Id);
        var cap = Assert.IsType<CapabilityStateOnOff>(Assert.Single(state.Capabilities));
        Assert.Equal(CapabilityStateOnOffInstance.On, cap.State.Instance);
        Assert.True(cap.State.Value);
    }

    [Fact]
    public void ToCommand_maps_onoff_action()
    {
        CapabilityActionParamsOnOff action = new()
        {
            State = new CapabilityStateOnOffData { Instance = CapabilityStateOnOffInstance.On, Value = true },
        };

        var command = Assert.IsType<OnOffCommand>(AliceMapper.ToCommand(action));
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
}
