namespace ThinkingHome.DeviceModel.Capabilities;

/// <summary>Вкл/выкл. Matter cluster On/Off (0x0006). Единственный instance — "on_off".</summary>
public sealed record OnOffCapability : Capability
{
    /// <summary>Канонический instance.</summary>
    public const string InstanceName = "on_off";
}
