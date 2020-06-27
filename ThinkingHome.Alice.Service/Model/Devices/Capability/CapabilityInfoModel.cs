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
}
