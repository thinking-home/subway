using System.Runtime.Serialization;

namespace ThinkingHome.Alice.Service.Model.Devices.Capability.ColorSetting
{
    public class ColorSettingCapabilityParams
    {
        public ColorModel? color_model { get; set; }

        public ColorTemperatureRange? temperature_k { get; set; }
    }

    public struct ColorTemperatureRange
    {
        public int min { get; set; }
        public int max { get; set; }
    }

    public enum ColorModel
    {
        [EnumMember(Value = "hsv")]
        HSV,

        [EnumMember(Value = "rgb")]
        RGB
    }
}