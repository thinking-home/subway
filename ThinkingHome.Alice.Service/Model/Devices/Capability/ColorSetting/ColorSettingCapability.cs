namespace ThinkingHome.Alice.Service.Model.Devices.Capability.ColorSetting
{
    public abstract class ColorSettingCapability<TState> : Capability<ColorSettingCapabilityParams, TState>
        where TState : CapabilityStateModelBase
    {
        protected sealed override CapabilityType Type { get; } = CapabilityType.ColorSetting;
        protected sealed override ColorSettingCapabilityParams Params => new ColorSettingCapabilityParams
        {
            color_model = ColorModel,
            temperature_k = TemperatureRange
        };
        protected abstract ColorModel? ColorModel { get; }
        protected virtual ColorTemperatureRange? TemperatureRange { get; } = null;
    }

    public abstract class RgbColorSettingCapability : ColorSettingCapability<RgbCapabilityState>
    {
        protected sealed override ColorModel? ColorModel { get; } = ColorSetting.ColorModel.RGB;
    }

    public class RgbCapabilityState: CapabilityStateModel<int>
    {
        public sealed override string instance { get; } = "rgb";
    }

    public abstract class HsvColorSettingCapability : ColorSettingCapability<HsvCapabilityState>
    {
        protected sealed override ColorModel? ColorModel { get; } = ColorSetting.ColorModel.HSV;
    }

    public class HsvCapabilityState: CapabilityStateModel<HsvColor>
    {
        public sealed override string instance { get; } = "hsv";
    }

    public struct HsvColor
    {
        public int h { get; set; }
        public int s { get; set; }
        public int v { get; set; }
    }
}