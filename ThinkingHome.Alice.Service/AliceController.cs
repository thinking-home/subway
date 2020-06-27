using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ThinkingHome.Alice.Service.Model;
using ThinkingHome.Alice.Service.Model.Devices;
using ThinkingHome.Alice.Service.Model.Devices.Capability.OnOff;

namespace ThinkingHome.Alice.Service
{
    // public class HsvCapabilityState : CapabilityStateModel<HsvCapabilityStateValue>
    // {
    //     public override string instance => "hsv";
    //
    //     public override HsvCapabilityStateValue value { get; set; }
    // }
    //
    // public class HsvCapabilityStateValue
    // {
    //     public int h { get; set; }
    //     public int s { get; set; }
    //     public int v { get; set; }
    // }

    public class TestBulbDefaultCapability : OnOffCapability
    {
        public bool Value { get; set; }

        protected override bool Retrievable => true;

        protected override bool SplitCommands => true;

        protected override bool GetValue() => Value;
    }

    public class TestBulb
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
            return new CapabilityActionResult
            {
                type = capabilityState.type,
                state = new CapabilityStateActionResult
                {
                    instance = capabilityState.state.instance,
                    action_result = new ActionResultModel {status = ActionResultStatus.DONE}
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


    public class AliceController : Controller
    {
        private TestBulb _bulb1 = new TestBulb("1");
        private TestBulb _bulb12 = new TestBulb("12");

        [HttpGet("/service/v1.0")]
        public ActionResult Index()
        {
            return Ok("moo");
        }

        [HttpGet("/service/v1.0/user/unlink")]
        public UnlinkResponse Unlink()
        {
            return new UnlinkResponse {request_id = "123"};
        }

        [HttpGet("/service/v1.0/user/devices")]
        public DevicesResponse Devices()
        {
            return new DevicesResponse
            {
                request_id = "123",
                payload = new DevicesPayload
                {
                    user_id = "dima117a",
                    devices = new[]
                    {
                        _bulb1.GetDescription(),
                        _bulb12.GetDescription(),
                    }
                }
            };
        }

        private TestBulb GetBulbById(string id)
        {
            if (id == _bulb1.Id)
            {
                return _bulb1;
            }

            if (id == _bulb12.Id)
            {
                return _bulb12;
            }

            return null;
        }

        [HttpPost("/service/v1.0/user/devices/query")]
        public DevicesQueryResponse DevicesQuery([FromBody] DevicesQueryRequest request)
        {
            var devices = request.devices.Select(d => GetBulbById(d.id).GetStateResponse()).ToArray();

            return new DevicesQueryResponse
            {
                request_id = "123",
                payload = new DevicesQueryPayload
                {
                    devices = devices
                }
            };
        }

        [HttpPost("/service/v1.0/user/devices/action")]
        public DevicesActionResponse DevicesAction([FromBody] DevicesActionRequest request)
        {
            DeviceActionResult MakeAction(DeviceAction action)
            {
                var bulb = GetBulbById(action.id);
                if (bulb == null)
                {
                    return new DeviceActionResult
                    {
                        id = action.id,
                        action_result = new ActionResultModel
                        {
                            status = ActionResultStatus.ERROR,
                            error_code = ActionResultErrorCode.DEVICE_NOT_FOUND
                        }
                    };
                }

                return bulb.MakeAction(action);
            }

            var devices = request.payload.devices.Select(MakeAction).ToArray();

            return new DevicesActionResponse
            {
                request_id = "1234",
                payload = new DevicesActionResponsePayload
                {
                    devices = devices
                }
            };
        }
    }

    public class DevicesActionResponse
    {
        public string request_id { get; set; }

        public DevicesActionResponsePayload payload { get; set; }
    }

    public class DevicesActionRequest
    {
        public DevicesActionRequestPayload payload { get; set; }
    }

    public class DevicesActionRequestPayload
    {
        public DeviceAction[] devices { get; set; }
    }

    public class DevicesActionResponsePayload
    {
        public DeviceActionResult[] devices { get; set; }
    }

    public class DeviceAction
    {
        public string id { get; set; }
        public object custom_data { get; set; }
        public CapabilityState[] capabilities { get; set; }
    }
}
