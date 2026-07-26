namespace ThinkingHome.DeviceModel.Properties;

/// <summary>Присутствие/движение (bool). Matter cluster Occupancy Sensing (0x0406). Instance — "occupancy".</summary>
public sealed record OccupancyProperty : Property
{
    /// <summary>Канонический instance.</summary>
    public const string InstanceName = "occupancy";
}
