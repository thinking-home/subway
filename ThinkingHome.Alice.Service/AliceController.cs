using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ThinkingHome.Alice.Service.Model;
using ThinkingHome.Alice.Service.Model.Devices;
using ThinkingHome.Alice.Service.Stub;

namespace ThinkingHome.Alice.Service
{
    public class AliceController : Controller
    {
        private readonly Dictionary<string, IDevice> _bulbs;

        public AliceController(Dictionary<string, IDevice> bulbs)
        {
            _bulbs = bulbs;
        }

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
                    devices = _bulbs.Values.Select(b => b.GetDescription()).ToArray()
                }
            };
        }

        private IDevice GetBulbById(string id)
        {
            return _bulbs.GetValueOrDefault(id);
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
