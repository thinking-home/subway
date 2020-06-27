namespace ThinkingHome.Alice.Service.Model.Devices.Capability.OnOff
{
    public abstract class OnOffCapability : Capability<OnOffCapabilityParams, OnCapabilityState>
    {
        protected override CapabilityType Type => CapabilityType.OnOff;
        protected abstract bool SplitCommands { get; }

        protected abstract bool GetValue();

        protected override OnOffCapabilityParams Params => new OnOffCapabilityParams {split = SplitCommands};

        protected override OnCapabilityState GetState() => new OnCapabilityState(GetValue());
    }
}