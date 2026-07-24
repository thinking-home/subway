using System.Text.Json;
using ThinkingHome.Alice.Mapping;
using ThinkingHome.Alice.Model.Capabilities.Range;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

public class AliceCallbackTests
{
    [Fact]
    public void Callback_state_request_serializes_with_yandex_field_names()
    {
        var request = AliceMapper.ToCallbackState("home",
        [
            new StateChange
            {
                DeviceId = "leak-1",
                Value = new WaterLeakState { Instance = "water_leak", Value = true },
            },
        ], ts: 1626202157);

        var json = JsonSerializer.Serialize(request);

        Assert.Contains("\"ts\":1626202157", json);
        Assert.Contains("\"user_id\":\"home\"", json);
        Assert.Contains("\"id\":\"leak-1#0\"", json);
        Assert.Contains("\"type\":\"devices.properties.event\"", json);
        Assert.Contains("\"value\":\"leak\"", json);
    }

    [Fact]
    public void Batch_dedups_slot_keeping_last_value()
    {
        // несколько изменений одного слота (устройство, endpoint, instance) за окно → уходит последнее
        var request = AliceMapper.ToCallbackState("home",
        [
            new StateChange { DeviceId = "dimmer-1", Value = new BrightnessState { Instance = "brightness", Value = 10 } },
            new StateChange { DeviceId = "dimmer-1", Value = new BrightnessState { Instance = "brightness", Value = 40 } },
            new StateChange { DeviceId = "dimmer-1", Value = new BrightnessState { Instance = "brightness", Value = 90 } },
        ], ts: 1);

        var device = Assert.Single(request.Payload.Devices);
        var state = Assert.IsType<CapabilityStateRange>(Assert.Single(device.Capabilities));
        Assert.Equal(90f, state.State.Value);
    }

    [Fact]
    public void Batch_dedups_shared_color_slot_keeping_last_representation()
    {
        // общий слот цвета: rgb и температура делят instance — побеждает последнее представление (как в кэше)
        var request = AliceMapper.ToCallbackState("home",
        [
            new StateChange { DeviceId = "rgb-1", Value = new ColorRgbState { Instance = ColorCapability.InstanceName, Value = 0xFF0000 } },
            new StateChange { DeviceId = "rgb-1", Value = new ColorTemperatureState { Instance = ColorCapability.InstanceName, Value = 4500 } },
        ], ts: 1);

        var device = Assert.Single(request.Payload.Devices);
        Assert.Single(device.Capabilities); // один слот — одно состояние, температурное
    }

    [Fact]
    public void Batch_groups_devices_and_endpoints_into_one_request()
    {
        // изменения разных устройств и endpoint'ов одного устройства → один запрос, разные devices[]
        var request = AliceMapper.ToCallbackState("home",
        [
            new StateChange { DeviceId = "switch-1", Value = new OnOffState { EndpointId = 0, Instance = "on_off", Value = true } },
            new StateChange { DeviceId = "switch-1", Value = new OnOffState { EndpointId = 1, Instance = "on_off", Value = false } },
            new StateChange { DeviceId = "climate-1", Value = new HumidityState { Instance = "humidity", Value = 45 } },
        ], ts: 1);

        Assert.Equal(new[] { "switch-1#0", "switch-1#1", "climate-1#0" },
            request.Payload.Devices.Select(d => d.Id).ToArray());
    }
}
