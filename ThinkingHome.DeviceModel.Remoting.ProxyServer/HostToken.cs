using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ThinkingHome.DeviceModel.Remoting.ProxyServer;

/// <summary>
/// Токены хостов. Прокси — единственный эмитент, один симметричный ключ подписи (HS256). Токен
/// коннектора долгоживущий (без <c>exp</c>); отзыв — сменой ключа. <c>hostId</c> лежит в claim.
/// </summary>
public static class HostToken
{
    public const string HostIdClaim = "hostId";
    public const string ConnectorAudience = "connector";
    public const string Issuer = "thinkinghome-proxy";

    public static string IssueConnectorToken(string signingKey, string hostId)
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = Issuer,
            Audience = ConnectorAudience,
            Claims = new Dictionary<string, object> { [HostIdClaim] = hostId },
            SigningCredentials = new SigningCredentials(Key(signingKey), SecurityAlgorithms.HmacSha256),
            // без Expires — долгоживущий; отзыв через смену ключа подписи
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }

    public static TokenValidationParameters ConnectorValidation(string signingKey) => new()
    {
        ValidateIssuer = true,
        ValidIssuer = Issuer,
        ValidateAudience = true,
        ValidAudience = ConnectorAudience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = Key(signingKey),
        ValidateLifetime = false, // токен коннектора без срока действия
    };

    private static SymmetricSecurityKey Key(string signingKey) => new(Encoding.UTF8.GetBytes(signingKey));
}
