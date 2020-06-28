using ThinkingHome.Alice.Service.Model.Devices.Capability.OnOff;

namespace ThinkingHome.Alice.Service.Stub
{
    public class TestBulbDefaultCapability : OnOffCapability
    {
        public bool Value { get; set; }

        protected override bool Retrievable => true;

        protected override bool SplitCommands => true;

        protected override bool GetValue() => Value;
    }
}