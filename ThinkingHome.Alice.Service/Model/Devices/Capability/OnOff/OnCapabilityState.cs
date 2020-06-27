namespace ThinkingHome.Alice.Service.Model.Devices.Capability.OnOff
{
    public class OnCapabilityState : CapabilityStateModel<bool>
    {
        public override string instance => "on";

        public override bool value { get; set; }
    }
}