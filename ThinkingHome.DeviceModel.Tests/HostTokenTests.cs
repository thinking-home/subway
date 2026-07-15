using Microsoft.IdentityModel.JsonWebTokens;
using ThinkingHome.DeviceModel.Remoting.ProxyServer;

namespace ThinkingHome.DeviceModel.Tests;

public class HostTokenTests
{
    private const string Key = "test-signing-key-at-least-32-bytes-long-xx";

    [Fact]
    public async Task Connector_token_validates_and_carries_hostId()
    {
        var token = HostToken.IssueConnectorToken(Key, "home-A");

        var result = await new JsonWebTokenHandler()
            .ValidateTokenAsync(token, HostToken.ConnectorValidation(Key));

        Assert.True(result.IsValid);
        Assert.Equal("home-A", result.Claims[HostToken.HostIdClaim].ToString());
    }

    [Fact]
    public async Task Token_signed_with_another_key_is_rejected()
    {
        var token = HostToken.IssueConnectorToken(Key, "home-A");

        var result = await new JsonWebTokenHandler()
            .ValidateTokenAsync(token, HostToken.ConnectorValidation("another-signing-key-at-least-32-bytes-x"));

        Assert.False(result.IsValid);
    }
}
