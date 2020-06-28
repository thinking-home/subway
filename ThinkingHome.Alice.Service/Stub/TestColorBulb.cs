using System.Linq;
using ThinkingHome.Alice.Service.Model;
using ThinkingHome.Alice.Service.Model.Devices;
using ThinkingHome.Alice.Service.Model.Devices.Capability.ColorSetting;
using ThinkingHome.Alice.Service.Model.Devices.Capability.OnOff;

namespace ThinkingHome.Alice.Service.Stub
{
    public class TestColorBulb: IDevice
    {
        private readonly string _id;
        private readonly TestBulbDefaultCapability _on;
        private readonly TestColorBulbHsvCapability _color;

        public TestColorBulb(string id)
        {
            _id = id;
            _on = new TestBulbDefaultCapability();
            _color = new TestColorBulbHsvCapability();
        }

        public string Id => _id;

        public DeviceState GetStateResponse()
        {
            return new DeviceState
            {
                id = _id,
                capabilities = new[] { _on.GetStateResponse(), _color.GetStateResponse() }
            };
        }

        public CapabilityActionResult SetCapabilityState(CapabilityState capabilityState)
        {
            var result = ActionResultModel.Ok;

            switch (capabilityState.type)
            {
                case CapabilityType.OnOff:
                    if (capabilityState.state is OnCapabilityState onoff)
                        _on.Value = onoff.value;
                    else
                        result = ActionResultModel.InvalidValue();
                    break;
                case CapabilityType.ColorSetting:
                    if (capabilityState.state is HsvCapabilityState hsv)
                        _color.Value = hsv.value;
                    else
                        result = ActionResultModel.InvalidValue();

                    break;
            }

            return capabilityState.GetActionResult(result);
        }

        public DeviceActionResult MakeAction(DeviceAction action)
        {
            var capabilities = action.capabilities.Select(SetCapabilityState).ToArray();

            return new DeviceActionResult
            {
                id = _id,
                capabilities = capabilities
            };
        }

        public DeviceModel GetDescription()
        {
            return new DeviceModel
            {
                id = _id,
                name = "Цветная лампочка",
                type = DeviceType.Light,
                capabilities = new[]
                {
                    _on.GetDescription(),
                    _color.GetDescription(),
                },
                device_info = new DeviceInfoModel
                {
                    manufacturer = "little cow",
                    model = "bulb2",
                    hw_version = "2",
                    sw_version = "377549"
                }
            };
        }
    }
}