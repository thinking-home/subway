namespace ThinkingHome.Alice.Service.Model.Devices.Capability.OnOff
{
    public abstract class OnOffCapability : Capability<OnOffCapabilityParams, OnCapabilityState>
    {
        protected sealed override CapabilityType Type => CapabilityType.OnOff;
        protected abstract bool SplitCommands { get; }

        protected abstract bool GetValue();

        protected sealed override OnOffCapabilityParams Params => new OnOffCapabilityParams {split = SplitCommands};

        protected sealed override OnCapabilityState GetState() => new OnCapabilityState {value = GetValue()};
    }
}