using System.Linq;
using ThinkingHome.Alice.Handlers.DevicesAction;
using ThinkingHome.Alice.Model;
using ThinkingHome.Alice.Model.ActionResult;
using ThinkingHome.Alice.Model.Capabilities;
using ThinkingHome.Alice.Model.Capabilities.OnOff;

namespace ThinkingHome.Alice.Service.Stub
{
    public class TestBulb(string id) : IDevice
    {
        public string Id => id;

        public bool Value = false;

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

        public DeviceActionResult MakeAction(DeviceActionParams action)
        {
            var capabilities = action.Capabilities.Select(SetCapabilityState).ToArray();

            return new DeviceActionResult
            {
                Id = Id,
                Capabilities = capabilities
            };
        }

        private CapabilityActionResultBase SetCapabilityState(CapabilityActionParamsBase obj)
        {
            var result = ActionResult.Ok;

            if (obj is CapabilityActionParamsOnOff actionParams)
            {
                // здесь не хватает выбора типа возможности
                Value = actionParams.State.Value;
            }
            else
            {
                result = ActionResult.InvalidValue();
            }

            return new CapabilityActionResultOnOff
            {
                State = new()
                {
                    Instance = CapabilityStateOnOffInstance.On,
                    ActionResult = result
                }
            };
        }
    }
}