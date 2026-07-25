namespace ThinkingHome.DeviceModel.Properties;

/// <summary>
/// Накопленные показания счётчика воды, м³ — нормализованная единица ядра. Вендорское расширение:
/// в Matter нет кластера учёта воды (Flow Measurement 0x0404 — мгновенный поток, не накопленный
/// объём). Instance — "water_meter".
/// </summary>
[VendorExtension]
public sealed record WaterMeterProperty : Property;
