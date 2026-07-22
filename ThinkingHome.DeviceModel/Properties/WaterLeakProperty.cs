namespace ThinkingHome.DeviceModel.Properties;

/// <summary>Протечка воды (bool, true — протечка обнаружена). Matter cluster Boolean State (0x0045) на типе Water Leak Detector (0x0043). Instance — "water_leak".</summary>
public sealed record WaterLeakProperty : Property;
