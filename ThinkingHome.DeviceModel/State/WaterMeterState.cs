namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущие накопленные показания счётчика воды, м³ (instance "water_meter"). Вендорское расширение (см. WaterMeterProperty).</summary>
[VendorExtension]
public sealed record WaterMeterState : StateValue
{
    public required double Value { get; init; }
}
