namespace ThinkingHome.DeviceModel.Properties;

/// <summary>Уровень заряда батареи, % (0–100) — нормализованная единица ядра (Matter отдаёт полу-проценты). Matter cluster Power Source (0x002F), атрибут BatPercentRemaining. Instance — "battery".</summary>
public sealed record BatteryProperty : Property;
