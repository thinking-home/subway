using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ThinkingHome.Alice.Service.Model.Devices.Capability;

namespace ThinkingHome.Alice.Service.Model.Devices
{
    public class DevicesPayload
    {
        // ReSharper disable InconsistentNaming
        public string user_id { get; set; }
        public DeviceModel[] devices { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public class DeviceModel
    {
        // ReSharper disable InconsistentNaming
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string room { get; set; }
        public DeviceType type { get; set; }
        public object custom_data { get; set; }
        public CapabilityInfoModel[] capabilities { get; set; }
        public PropertyModel[] properties { get; set; }

        public DeviceInfoModel device_info { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public class PropertyModel
    {
    }

    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum DeviceType
    {
        [EnumMember(Value = "devices.types.light")]
        Light,

        [EnumMember(Value = "devices.types.socket")]
        Socket,

        [EnumMember(Value = "devices.types.switch")]
        Switch,

        [EnumMember(Value = "devices.types.thermostat")]
        Thermostat,

        [EnumMember(Value = "devices.types.thermostat.ac")]
        ThermostatAC,
    }
}