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

    [JsonStringEnumMemberName("devices.types.switch")]
    Switch,

    [JsonStringEnumMemberName("devices.types.openable.curtain")]
    Curtain,

    [JsonStringEnumMemberName("devices.types.ventilation.fan")]
    Fan,

    [JsonStringEnumMemberName("devices.types.thermostat.ac")]
    ThermostatAc,
}