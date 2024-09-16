using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ThinkingHome.Alice.Handlers;
using ThinkingHome.Alice.Handlers.Devices;
using ThinkingHome.Alice.Handlers.DevicesAction;
using ThinkingHome.Alice.Handlers.DevicesQuery;
using ThinkingHome.Alice.Model;
using ThinkingHome.Alice.Model.ActionResult;
using ThinkingHome.Alice.Service.Stub;
using ActionResult = ThinkingHome.Alice.Model.ActionResult.ActionResult;

namespace ThinkingHome.Alice.Service
{
    [Route("/service/v1.0")]
    public class AliceController(Dictionary<string, IDevice> bulbs) : Controller
    {
        [HttpGet, HttpHead]
        public Microsoft.AspNetCore.Mvc.ActionResult Index()
        {
            return Ok("moo");
        }

        [HttpGet("user/unlink")]
        public UnlinkResponse Unlink([FromHeader(Name = "X-Request-Id")] string requestId)
        {
            return new UnlinkResponse
            {
                RequestId = requestId ?? Guid.NewGuid().ToString("N"),
            };
        }

        [HttpGet("user/devices")]
        public DevicesResponse Devices([FromHeader(Name = "X-Request-Id")] string requestId)
        {
            return new DevicesResponse
            {
                RequestId = requestId ?? Guid.NewGuid().ToString("N"),
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
            [FromHeader(Name = "X-Request-Id")] string requestId,
            [FromBody] DevicesQueryRequest request)
        {
            var devices = request.Devices.Select(d => GetBulbById(d.Id).GetStateResponse()).ToArray();

            return new DevicesQueryResponse
            {
                RequestId = requestId ?? Guid.NewGuid().ToString("N"),
                Payload = new DevicesQueryPayload
                {
                    Devices = devices
                }
            };
        }

        [HttpPost("user/devices/action")]
        public DevicesActionResponse DevicesAction(
            [FromHeader(Name = "X-Request-Id")] string requestId,
            [FromBody] DevicesActionRequest request)
        {
            DeviceActionResult MakeAction(DeviceActionParams action)
            {
                var bulb = GetBulbById(action.Id);
                if (bulb == null)
                {
                    return new DeviceActionResult
                    {
                        Id = action.Id,
                        ActionResult = new ActionResult
                        {
                            Status = ActionResultStatus.ERROR,
                            ErrorCode = ActionResultErrorCode.DEVICE_NOT_FOUND
                        }
                    };
                }

                return bulb.MakeAction(action);
            }

            var devices = request.Payload.Devices.Select(MakeAction).ToArray();

            return new DevicesActionResponse
            {
                RequestId = requestId ?? Guid.NewGuid().ToString("N"),
                Payload = new DevicesActionPayload
                {
                    Devices = devices
                }
            };
        }
    }
}