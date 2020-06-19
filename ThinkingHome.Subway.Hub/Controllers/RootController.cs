using Microsoft.AspNetCore.Mvc;
using ThinkingHome.Alice.Service;

namespace ThinkingHome.Subway.Hub.Controllers
{
    [Route("service")]
    public class RootController : AliceController
    {
        [HttpGet("test")]
        public ActionResult Test()
        {
            return Json(new {it = "works!"});
        }
    }
}