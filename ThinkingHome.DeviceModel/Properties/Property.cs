using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel.Properties;

/// <summary>
/// Свойство устройства (сенсор, только чтение). Закрытая иерархия, как и у способностей.
/// При добавлении наследника обязательно зарегистрировать его в [JsonDerivedType] ниже.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(TemperatureProperty), "temperature")]
[JsonDerivedType(typeof(HumidityProperty), "humidity")]
[JsonDerivedType(typeof(OccupancyProperty), "occupancy")]
[JsonDerivedType(typeof(ContactProperty), "contact")]
[JsonDerivedType(typeof(WaterLeakProperty), "waterLeak")]
[JsonDerivedType(typeof(BatteryProperty), "battery")]
public abstract record Property
{
    /// <summary>Экземпляр свойства (например, "temperature", "motion").</summary>
    public required string Instance { get; init; }

    public bool Retrievable { get; init; } = true;
    public bool Reportable { get; init; } = true;
}
