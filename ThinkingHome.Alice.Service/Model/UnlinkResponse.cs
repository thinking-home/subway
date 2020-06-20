using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Service.Model
{
    public class UnlinkResponse
    {
        public string request_id { get; set; }
    }

    public class DevicesResponse
    {
        public string request_id { get; set; }

        public DevicesPayload payload { get; set; }
    }

    public class DevicesPayload
    {
        public string user_id { get; set; }

        public DeviceModel[] devices { get; set; }
    }

    public class DeviceModel
    {
        public string id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string room { get; set; }

        public DeviceType type { get; set; }

        public object custom_data { get; set; }

        public CapabilityModel[] capabilities { get; set; }

        public PropertyModel[] properties { get; set; }

        public DeviceInfoModel device_info { get; set; }
    }

    public class DeviceInfoModel
    {
        public string manufacturer { get; set; }
        public string model { get; set; }
        public string hw_version { get; set; }
        public string sw_version { get; set; }
    }

    public class PropertyModel
    {
    }

    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum CapabilityType
    {
        [EnumMember(Value = "devices.capabilities.on_off")]
        OnOff,

        [EnumMember(Value = "devices.capabilities.color_setting")]
        ColorSetting,

        [EnumMember(Value = "devices.capabilities.mode")]
        Mode,

        [EnumMember(Value = "devices.capabilities.range")]
        Range,

        [EnumMember(Value = "devices.capabilities.toggle")]
        Toggle
    }

    public class OnOffCapabilityModel: CapabilityModel
    {
        public override CapabilityType type { get; } = CapabilityType.OnOff;

        public bool split { get; set; }

        public override object parameters => new {split};
    }

    public abstract class CapabilityModel
    {
        public abstract CapabilityType type { get; }

        public bool retrievable { get; set; }

        public abstract object parameters { get; }
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