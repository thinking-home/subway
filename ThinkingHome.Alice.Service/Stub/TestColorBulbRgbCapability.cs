using ThinkingHome.Alice.Service.Model.Devices.Capability.ColorSetting;

namespace ThinkingHome.Alice.Service.Stub
{
    public class TestColorBulbHsvCapability: HsvColorSettingCapability
    {
        public HsvColor Value { get; set; }

        protected override bool Retrievable { get; } = true;
        protected override HsvCapabilityState GetState()
        {
            return new HsvCapabilityState
            {
                value = Value
            };
        }
    }
}