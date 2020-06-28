namespace ThinkingHome.Alice.Service.Model.Devices.Capability
{
    public abstract class CapabilityStateModelBase
    {
        public abstract string instance { get; }

        public abstract object SerializeValue();

    }

    public abstract class CapabilityStateModel<TValue> : CapabilityStateModelBase
    {
        public TValue value { get; set; }

        public override object SerializeValue() => value;
    }
}
