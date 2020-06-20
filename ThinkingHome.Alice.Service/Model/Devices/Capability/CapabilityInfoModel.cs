using System.Security.Cryptography.X509Certificates;

namespace ThinkingHome.Alice.Service.Model.Devices.Capability
{
    public class CapabilityInfoModel
    {
        // ReSharper disable InconsistentNaming
        public CapabilityType type { get; set; }
        public bool retrievable { get; set; }
        public object parameters { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public abstract class CapabilityStateModel
    {
        public abstract string instance { get; }
        public abstract object value { get; }
    }

    public class OnOffCapabilityStateModel: CapabilityStateModel
    {
        public OnOffCapabilityStateModel(bool value)
        {
            this.value = value;
        }

        public override string instance { get; } = "on";

        public override object value { get; }
    }
}