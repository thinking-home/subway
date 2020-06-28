using System.Linq;
using ThinkingHome.Alice.Service.Model;
using ThinkingHome.Alice.Service.Model.Devices;
using ThinkingHome.Alice.Service.Model.Devices.Capability.OnOff;

namespace ThinkingHome.Alice.Service.Stub
{
    public interface IDevice
    {
        string Id { get; }
        DeviceState GetStateResponse();
        DeviceActionResult MakeAction(DeviceAction action);
        DeviceModel GetDescription();
    }

    public class TestBulb: IDevice
    {
        private readonly string _id;
        private readonly TestBulbDefaultCapability _xxx;

        public TestBulb(string id)
        {
            _id = id;
            _xxx = new TestBulbDefaultCapability();
        }

        public string Id => _id;

        public DeviceState GetStateResponse()
        {
            return new DeviceState
            {
                id = _id,
                capabilities = new[] {_xxx.GetStateResponse()}
            };
        }

        public CapabilityActionResult SetCapabilityState(CapabilityState capabilityState)
        {
            var result = new ActionResultModel {status = ActionResultStatus.DONE};
            var onoff = capabilityState.state as OnCapabilityState;
            if (onoff == null)
            {
                result = new ActionResultModel
                {
                    status = ActionResultStatus.ERROR,
                    error_code = ActionResultErrorCode.INVALID_VALUE
                };
            }
            else
            {
                _xxx.Value = onoff.value;
            }

            return new CapabilityActionResult
            {
                type = capabilityState.type,
                state = new CapabilityStateActionResult
                {
                    instance = capabilityState.state.instance,
                    action_result = result
                }
            };
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
                name = "Лампочка",
                type = DeviceType.Light,
                capabilities = new[] {_xxx.GetDescription()},
                device_info = new DeviceInfoModel
                {
                    manufacturer = "little cow",
                    model = "bulb1",
                    hw_version = "1",
                    sw_version = "377549"
                }
            };
        }
    }
}