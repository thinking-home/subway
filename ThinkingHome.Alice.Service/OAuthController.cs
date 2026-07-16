using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using ThinkingHome.DeviceModel.Remoting.ProxyServer;

namespace ThinkingHome.Alice.Service
{
    // OAuth-сервер (authorization code). Прокси stateless: код и access token — JWT, сессий/хранилищ нет.
    // hostId/redirect_uri/state между шагами формы — в скрытых полях. OTP живёт на хосте.
    public class OAuthController(RemoteHostRegistry registry, IConfiguration configuration) : Controller
    {
        private string SigningKey => configuration["Jwt:SigningKey"]
            ?? throw new InvalidOperationException("Jwt:SigningKey не задан.");

        [HttpGet("/oauth/authorize")]
        public IActionResult Authorize(string redirect_uri, string state)
            => Html(HostIdForm(redirect_uri, state, message: null));

        [HttpPost("/oauth/authorize")]
        public async Task<IActionResult> Authorize(string redirect_uri, string state, string hostId, string otp)
        {
            if (string.IsNullOrEmpty(otp))
            {
                // шаг 1: просим хост сгенерировать OTP (в его лог)
                try
                {
                    await registry.GenerateLinkingOtpAsync(hostId);
                }
                catch (HostUnavailableException)
                {
                    return Html(HostIdForm(redirect_uri, state, $"Хост «{hostId}» не в сети."));
                }

                return Html(OtpForm(redirect_uri, state, hostId, "Код показан в логе хоста — введите его."));
            }

            // шаг 2: проверяем OTP на хосте
            bool valid;
            try
            {
                valid = await registry.ValidateLinkingOtpAsync(hostId, otp);
            }
            catch (HostUnavailableException)
            {
                return Html(HostIdForm(redirect_uri, state, $"Хост «{hostId}» не в сети."));
            }

            if (!valid)
            {
                return Html(OtpForm(redirect_uri, state, hostId, "Неверный код, попробуйте ещё раз."));
            }

            var code = HostToken.IssueCode(SigningKey, hostId);
            var url = QueryHelpers.AddQueryString(redirect_uri, "code", code);
            url = QueryHelpers.AddQueryString(url, "state", state ?? "");
            return Redirect(url);
        }

        [HttpPost("/oauth/token")]
        public async Task<IActionResult> Token([FromForm] string code)
        {
            var hostId = await HostToken.TryReadCodeHostIdAsync(SigningKey, code);
            if (hostId is null)
            {
                return BadRequest(new { error = "invalid_grant" });
            }

            return Json(new { access_token = HostToken.IssueAccessToken(SigningKey, hostId), token_type = "bearer" });
        }

        private IActionResult Html(string body) => Content(body, "text/html; charset=utf-8");

        private static string HostIdForm(string redirectUri, string state, string message) => $$"""
            <!doctype html><meta charset="utf-8"><title>Привязка</title>
            <form method="post" action="/oauth/authorize">{{Msg(message)}}
            <p>Привязка дома к Алисе. Введите id хоста:</p>
            <input name="hostId" placeholder="hostId" autofocus>
            <input type="hidden" name="redirect_uri" value="{{Enc(redirectUri)}}">
            <input type="hidden" name="state" value="{{Enc(state)}}">
            <button type="submit">Далее</button></form>
            """;

        private static string OtpForm(string redirectUri, string state, string hostId, string message) => $$"""
            <!doctype html><meta charset="utf-8"><title>Код привязки</title>
            <form method="post" action="/oauth/authorize">{{Msg(message)}}
            <p>Одноразовый код показан в логе хоста «{{Enc(hostId)}}». Введите его:</p>
            <input name="otp" placeholder="код" autofocus>
            <input type="hidden" name="hostId" value="{{Enc(hostId)}}">
            <input type="hidden" name="redirect_uri" value="{{Enc(redirectUri)}}">
            <input type="hidden" name="state" value="{{Enc(state)}}">
            <button type="submit">Привязать</button></form>
            """;

        private static string Msg(string message)
            => string.IsNullOrEmpty(message) ? "" : $"<p style=\"color:#b00\">{Enc(message)}</p>";

        private static string Enc(string value) => WebUtility.HtmlEncode(value ?? "");
    }
}
