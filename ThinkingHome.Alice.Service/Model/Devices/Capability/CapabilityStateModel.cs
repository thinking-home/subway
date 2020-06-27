namespace ThinkingHome.Alice.Service.Model.Devices.Capability
{
    public abstract class CapabilityStateModelBase
    {
        public abstract string instance { get; }

        public abstract object SerializeValue();

    }

    public abstract class CapabilityStateModel : CapabilityStateModelBase
    {
        public abstract object value { get; }

        public override object SerializeValue() => value;
    }
}
