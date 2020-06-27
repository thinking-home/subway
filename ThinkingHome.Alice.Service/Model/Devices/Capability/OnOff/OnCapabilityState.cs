namespace ThinkingHome.Alice.Service.Model.Devices.Capability.OnOff
{
    public class OnCapabilityState : CapabilityStateModel
    {
        public OnCapabilityState(bool value)
        {
            this.value = value;
        }

        public override string instance => "on";

        public override object value { get; }
    }
}