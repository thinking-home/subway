using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ThinkingHome.DeviceModel.Remoting.ProxyServer;

/// <summary>
/// JWT прокси. Прокси — единственный эмитент, один симметричный ключ (HS256). Стейт не хранится:
/// токены самодостаточны, отзыв — сменой ключа. Аудитории: connector (коннектор), authcode (OAuth-код),
/// alice (access token). Во всех токенах — hostId в claim.
/// </summary>
public static class HostToken
{
    public const string HostIdClaim = "hostId";
    public const string Issuer = "thinkinghome-proxy";

    public const string ConnectorAudience = "connector";
    public const string CodeAudience = "authcode";
    public const string AliceAudience = "alice";

    public const string ConnectorScheme = "Connector";
    public const string AliceScheme = "Alice";

    public static string IssueConnectorToken(string signingKey, string hostId)
        => Issue(signingKey, hostId, ConnectorAudience, expires: null);

    public static string IssueCode(string signingKey, string hostId)
        => Issue(signingKey, hostId, CodeAudience, DateTime.UtcNow.AddMinutes(1));

    public static string IssueAccessToken(string signingKey, string hostId)
        => Issue(signingKey, hostId, AliceAudience, expires: null);

    public static TokenValidationParameters ConnectorValidation(string signingKey)
        => Validation(signingKey, ConnectorAudience, validateLifetime: false);

    public static TokenValidationParameters AliceValidation(string signingKey)
        => Validation(signingKey, AliceAudience, validateLifetime: false);

    /// <summary>Проверить OAuth-код и вернуть hostId (или null, если код невалиден/просрочен).</summary>
    public static async Task<string?> TryReadCodeHostIdAsync(string signingKey, string code)
    {
        var result = await new JsonWebTokenHandler()
            .ValidateTokenAsync(code, Validation(signingKey, CodeAudience, validateLifetime: true));
        return result.IsValid ? result.Claims[HostIdClaim].ToString() : null;
    }

    private static string Issue(string signingKey, string hostId, string audience, DateTime? expires)
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = Issuer,
            Audience = audience,
            Claims = new Dictionary<string, object> { [HostIdClaim] = hostId },
            Expires = expires,
            SigningCredentials = new SigningCredentials(Key(signingKey), SecurityAlgorithms.HmacSha256),
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }

    private static TokenValidationParameters Validation(string signingKey, string audience, bool validateLifetime) => new()
    {
        ValidateIssuer = true,
        ValidIssuer = Issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = Key(signingKey),
        ValidateLifetime = validateLifetime,
    };

    private static SymmetricSecurityKey Key(string signingKey) => new(Encoding.UTF8.GetBytes(signingKey));
}
