namespace ThinkingHome.DeviceModel.Properties;

/// <summary>Атмосферное давление, кПа — нормализованная единица ядра (единица Matter). Перевод в единицы экосистемы (мм рт. ст. у Алисы) — в адаптере. Matter cluster Pressure Measurement (0x0403). Instance — "pressure".</summary>
public sealed record PressureProperty : Property;
