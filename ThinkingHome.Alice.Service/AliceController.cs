using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ThinkingHome.Alice.Handlers.Devices;
using ThinkingHome.Alice.Service.Model;
using ThinkingHome.Alice.Service.Model.Devices;
using ThinkingHome.Alice.Service.Stub;

namespace ThinkingHome.Alice.Service
{
    [Route("/service/v1.0")]
    public class AliceController(Dictionary<string, IDevice> bulbs) : Controller
    {
        [HttpGet, HttpHead]
        public ActionResult Index()
        {
            return Ok("moo");
        }

        [HttpGet("user/unlink")]
        public UnlinkResponse Unlink([FromHeader(Name = "X-Request-Id")] string request_id)
        {
            return new UnlinkResponse
            {
                request_id = request_id ?? Guid.NewGuid().ToString("N"),
            };
        }

        [HttpGet("user/devices")]
        public DevicesResponse Devices([FromHeader(Name = "X-Request-Id")] string request_id)
        {
            return new DevicesResponse
            {
                RequestId = request_id ?? Guid.NewGuid().ToString("N"),
                Payload = new DevicesPayload
                {
                    UserId = "dima117a",
                    Devices = bulbs.Values.Select(b => b.GetDescription()).ToArray()
                }
            };
        }

        private IDevice GetBulbById(string id)
        {
            return bulbs.GetValueOrDefault(id);
        }

        [HttpPost("user/devices/query")]
        public DevicesQueryResponse DevicesQuery(
            [FromHeader(Name = "X-Request-Id")] string request_id,
            [FromBody] DevicesQueryRequest request)
        {
            var devices = request.devices.Select(d => GetBulbById(d.id).GetStateResponse()).ToArray();

            return new DevicesQueryResponse
            {
                request_id = request_id ?? Guid.NewGuid().ToString("N"),
                payload = new DevicesQueryPayload
                {
                    devices = devices
                }
            };
        }

        [HttpPost("user/devices/action")]
        public DevicesActionResponse DevicesAction(
            [FromHeader(Name = "X-Request-Id")] string request_id,
            [FromBody] DevicesActionRequest request)
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
                request_id = request_id ?? Guid.NewGuid().ToString("N"),
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
        // public CapabilityState[] capabilities { get; set; }
    }
}