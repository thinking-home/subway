using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeviceType
{
    [JsonStringEnumMemberName("devices.types.light")]
    Light,

    [JsonStringEnumMemberName("devices.types.socket")]
    Socket,
}