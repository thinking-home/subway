using System;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace ThinkingHome.Alice.Service
{
    public class OAuthController : Controller
    {
        [HttpGet("/oauth/authorize")]
        public IActionResult Index(string redirect_uri, string state)
        {
            var url = new UriBuilder(redirect_uri);
            var qs = HttpUtility.ParseQueryString(url.Query);
            qs["code"] = Guid.NewGuid().ToString("N");

            if (!string.IsNullOrEmpty(state)) qs["state"] = state;

            url.Query = qs.ToString();

            return Redirect(url.ToString());
        }

        [HttpPost("/oauth/token")]
        public IActionResult Token(string code)
        {
            Console.WriteLine("Code: {0}", code);

            return Json(new
            {
                access_token = Guid.Empty.ToString("N"),
                token_type = "bearer",
            });
        }
    }
}
