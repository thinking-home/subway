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
    /// <summary>Имя claim'а с идентификатором домашнего хоста.</summary>
    public const string HostIdClaim = "hostId";
    /// <summary>Издатель всех токенов прокси.</summary>
    public const string Issuer = "thinkinghome-proxy";

    /// <summary>Аудитория токена коннектора (постоянный доступ дома к хабу).</summary>
    public const string ConnectorAudience = "connector";
    /// <summary>Аудитория одноразового OAuth-кода.</summary>
    public const string CodeAudience = "authcode";
    /// <summary>Аудитория access-токена Алисы.</summary>
    public const string AliceAudience = "alice";

    /// <summary>Имя схемы аутентификации коннектора.</summary>
    public const string ConnectorScheme = "Connector";
    /// <summary>Имя схемы аутентификации запросов Алисы.</summary>
    public const string AliceScheme = "Alice";

    /// <summary>Выпустить токен коннектора для hostId (lifetime при проверке не смотрится).</summary>
    public static string IssueConnectorToken(string signingKey, string hostId)
        => Issue(signingKey, hostId, ConnectorAudience, expires: null);

    /// <summary>Выпустить одноразовый OAuth-код для hostId (TTL 1 минута).</summary>
    public static string IssueCode(string signingKey, string hostId)
        => Issue(signingKey, hostId, CodeAudience, DateTime.UtcNow.AddMinutes(1));

    /// <summary>Выпустить access-токен Алисы для hostId.</summary>
    public static string IssueAccessToken(string signingKey, string hostId)
        => Issue(signingKey, hostId, AliceAudience, expires: null);

    /// <summary>Параметры валидации токена коннектора (только подпись и аудитория).</summary>
    public static TokenValidationParameters ConnectorValidation(string signingKey)
        => Validation(signingKey, ConnectorAudience, validateLifetime: false);

    /// <summary>Параметры валидации access-токена Алисы (только подпись и аудитория).</summary>
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
