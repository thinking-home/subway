using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThinkingHome.Alice.Handlers;
using ThinkingHome.Alice.Handlers.Devices;
using ThinkingHome.Alice.Handlers.DevicesAction;
using ThinkingHome.Alice.Handlers.DevicesQuery;
using ThinkingHome.Alice.Mapping;
using ThinkingHome.Alice.Model.ActionResult;
using ThinkingHome.Alice.Model.Capabilities;
using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Remoting.ProxyServer;
using AliceActionResult = ThinkingHome.Alice.Model.ActionResult.ActionResult;
using AliceDeviceState = ThinkingHome.Alice.Model.DeviceState;

namespace ThinkingHome.Alice.Service
{
    [Route("/service/v1.0")]
    public class AliceController(IRemoteHostRegistry registry, IHostIdResolver hostIds) : Controller
    {
        [HttpGet, HttpHead]
        public IActionResult Index() => Ok("moo");

        [HttpGet("user/unlink")]
        public UnlinkResponse Unlink([FromHeader(Name = "X-Request-Id")] string requestId)
            => new() { RequestId = requestId ?? NewId() };

        [HttpGet("user/devices")]
        public async Task<DevicesResponse> Devices([FromHeader(Name = "X-Request-Id")] string requestId)
        {
            var hostId = hostIds.Resolve(HttpContext);

            var devices = registry.TryGet(hostId, out var host)
                ? (await host.GetDevicesAsync()).Select(AliceMapper.ToDevice).ToArray()
                : [];

            return new DevicesResponse
            {
                RequestId = requestId ?? NewId(),
                Payload = new DevicesPayload { UserId = hostId, Devices = devices },
            };
        }

        [HttpPost("user/devices/query")]
        public async Task<DevicesQueryResponse> DevicesQuery(
            [FromHeader(Name = "X-Request-Id")] string requestId,
            [FromBody] DevicesQueryRequest request)
        {
            registry.TryGet(hostIds.Resolve(HttpContext), out var host);

            var states = new List<AliceDeviceState>();
            foreach (var reference in request.Devices)
            {
                states.Add(await QueryDevice(host, reference.Id));
            }

            return new DevicesQueryResponse
            {
                RequestId = requestId ?? NewId(),
                Payload = new DevicesQueryPayload { Devices = states.ToArray() },
            };
        }

        [HttpPost("user/devices/action")]
        public async Task<DevicesActionResponse> DevicesAction(
            [FromHeader(Name = "X-Request-Id")] string requestId,
            [FromBody] DevicesActionRequest request)
        {
            registry.TryGet(hostIds.Resolve(HttpContext), out var host);

            var devices = new List<DeviceActionResult>();
            foreach (var action in request.Payload.Devices)
            {
                devices.Add(await ExecuteDevice(host, action));
            }

            return new DevicesActionResponse
            {
                RequestId = requestId ?? NewId(),
                Payload = new DevicesActionPayload { Devices = devices.ToArray() },
            };
        }

        // ── per-device orchestration (маппер чистый, вызовы хоста — здесь) ──

        private static async Task<AliceDeviceState> QueryDevice(IDeviceHost host, string deviceId)
        {
            if (host == null) return UnreachableState(deviceId);

            try
            {
                return AliceMapper.ToDeviceState(await host.QueryAsync(deviceId));
            }
            catch (Exception)
            {
                return UnreachableState(deviceId);
            }
        }

        private static async Task<DeviceActionResult> ExecuteDevice(IDeviceHost host, DeviceActionParams action)
        {
            if (host == null) return UnreachableAction(action.Id);

            try
            {
                var capabilities = new List<CapabilityActionResultBase>();
                foreach (var capability in action.Capabilities)
                {
                    var outcome = await host.ExecuteAsync(action.Id, AliceMapper.ToCommand(capability));
                    capabilities.Add(AliceMapper.ToCapabilityActionResult(capability, outcome));
                }

                return new DeviceActionResult { Id = action.Id, Capabilities = capabilities.ToArray() };
            }
            catch (Exception)
            {
                return UnreachableAction(action.Id);
            }
        }

        private static AliceDeviceState UnreachableState(string deviceId) => new()
        {
            Id = deviceId,
            ErrorCode = ActionResultErrorCode.DEVICE_UNREACHABLE.ToString(),
            ErrorMessage = "device host is not connected",
        };

        private static DeviceActionResult UnreachableAction(string deviceId) => new()
        {
            Id = deviceId,
            ActionResult = new AliceActionResult
            {
                Status = ActionResultStatus.ERROR,
                ErrorCode = ActionResultErrorCode.DEVICE_UNREACHABLE,
            },
        };

        private static string NewId() => Guid.NewGuid().ToString("N");
    }
}
