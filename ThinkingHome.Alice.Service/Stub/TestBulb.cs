using System.Linq;
using ThinkingHome.Alice.Model;
using ThinkingHome.Alice.Model.Capabilities;
using ThinkingHome.Alice.Service.Model;
using ThinkingHome.Alice.Service.Model.Devices;

namespace ThinkingHome.Alice.Service.Stub
{
    public interface IDevice
    {
        string Id { get; }
        DeviceState GetStateResponse();
        DeviceActionResult MakeAction(DeviceAction action);
        Device GetDescription();
    }

    public class TestBulb(string id) : IDevice
    {
        public string Id => id;

        public DeviceState GetStateResponse()
        {
            return new DeviceState
            {
                id = id,
                // capabilities = new[] {_xxx.GetStateResponse()}
            };
        }

        public DeviceActionResult MakeAction(DeviceAction action)
        {
            throw new System.NotImplementedException();
        }

        // public CapabilityActionResult SetCapabilityState(CapabilityState capabilityState)
        // {
        //     var result = new ActionResultModel {status = ActionResultStatus.DONE};
        //     var onoff = capabilityState.state as OnCapabilityState;
        //     if (onoff == null)
        //     {
        //         result = new ActionResultModel
        //         {
        //             status = ActionResultStatus.ERROR,
        //             error_code = ActionResultErrorCode.INVALID_VALUE
        //         };
        //     }
        //     else
        //     {
        //         _xxx.Value = onoff.value;
        //     }
        //
        //     return new CapabilityActionResult
        //     {
        //         type = capabilityState.type,
        //         state = new CapabilityStateActionResult
        //         {
        //             instance = capabilityState.state.instance,
        //             action_result = result
        //         }
        //     };
        // }

        // public DeviceActionResult MakeAction(DeviceAction action)
        // {
        //     var capabilities = action.capabilities.Select(SetCapabilityState).ToArray();
        //
        //     return new DeviceActionResult
        //     {
        //         id = _id,
        //         capabilities = capabilities
        //     };
        // }

        public Device GetDescription()
        {
            var obj1 = new CapabilityInfoOnOff
            {
                reportable = false,
                retrievable = true,
                parameters = new()
                {
                    split = true,
                }
            };

            return new Device
            {
                id = Id,
                name = "Лампочка",
                type = DeviceType.Light,
                capabilities =
                [
                    new CapabilityInfoOnOff
                    {
                        reportable = true,
                        retrievable = true,
                        parameters = new() { split = true }
                    }
                ],
                device_info = new DeviceInfo
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