using Microsoft.AspNetCore.Mvc;
using ThinkingHome.Alice.Service.Model;

namespace ThinkingHome.Alice.Service
{
    public class AliceController : Controller
    {
        [HttpGet("/service")]
        public ActionResult Index()
        {
            return Ok("moo");
        }

        [HttpGet("/service/user/unlink")]
        public UnlinkResponse Unlink()
        {
            return new UnlinkResponse {request_id = "123"};
        }

        [HttpGet("/service/user/devices")]
        public DevicesResponse Devices()
        {
            return new DevicesResponse
            {
                request_id = "123",
                payload = new DevicesPayload
                {
                    user_id = "dima117a",
                    devices = new []{ GetBulb() }
                }
            };
        }

        private DeviceModel GetBulb()
        {
            return new DeviceModel
            {
                id = "1",
                name = "Лампочка",
                type = DeviceType.Light,
                capabilities = new CapabilityModel[]
                {
                    new OnOffCapabilityModel { retrievable = true, split = true }
                },
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