using System.Runtime.Serialization;

namespace ThinkingHome.Alice.Service.Model.Devices
{
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
}