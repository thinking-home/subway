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
                Id = id,
                Capabilities =
                [
                    new CapabilityStateOnOff
                    {
                        State = new()
                        {
                            Instance = CapabilityStateOnOffInstance.On,
                            Value = false
                        }
                    }
                ]
            };
        }

        public DeviceActionResult MakeAction(DeviceAction action)
        {
            return new DeviceActionResult
            {
                id = Id,
                capabilities = []
            }; 
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
            return new Device
            {
                Id = Id,
                Name = "Лампочка",
                Type = DeviceType.Light,
                Capabilities =
                [
                    new CapabilityInfoOnOff
                    {
                        Reportable = true,
                        Retrievable = true,
                        Parameters = new() { Split = true }
                    }
                ],
                DeviceInfo = new DeviceInfo
                {
                    Manufacturer = "little cow",
                    Model = "bulb1",
                    HardwareVersion = "1",
                    SoftwareVersion = "377549"
                }
            };
        }
    }
}