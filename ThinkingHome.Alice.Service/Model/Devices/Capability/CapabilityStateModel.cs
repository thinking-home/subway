namespace ThinkingHome.Alice.Service.Model.Devices.Capability
{
    public abstract class CapabilityStateModel
    {
        public abstract string instance { get; }
        public abstract object value { get; }
    }
}
