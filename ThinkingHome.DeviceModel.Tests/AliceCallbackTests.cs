using System.Text.Json;
using ThinkingHome.Alice.Mapping;
using ThinkingHome.Alice.Model.Callback;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

public class AliceCallbackTests
{
    [Fact]
    public void Callback_state_request_serializes_with_yandex_field_names()
    {
        var request = new CallbackStateRequest
        {
            Ts = 1626202157,
            Payload = new CallbackStatePayload
            {
                UserId = "home",
                Devices =
                [
                    AliceMapper.ToDeviceState(new StateChange
                    {
                        DeviceId = "leak-1",
                        Value = new WaterLeakState { Instance = "water_leak", Value = true },
                    }),
                ],
            },
        };

        var json = JsonSerializer.Serialize(request);

        Assert.Contains("\"ts\":1626202157", json);
        Assert.Contains("\"user_id\":\"home\"", json);
        Assert.Contains("\"id\":\"leak-1#0\"", json);
        Assert.Contains("\"type\":\"devices.properties.event\"", json);
        Assert.Contains("\"value\":\"leak\"", json);
    }
}
