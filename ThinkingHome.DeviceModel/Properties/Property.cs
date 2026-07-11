namespace ThinkingHome.DeviceModel.Properties;

/// <summary>
/// Свойство устройства (сенсор, только чтение). Закрытая иерархия, как и у способностей.
/// </summary>
public abstract record Property
{
    /// <summary>Экземпляр свойства (например, "temperature", "motion").</summary>
    public required string Instance { get; init; }

    public bool Retrievable { get; init; } = true;
    public bool Reportable { get; init; } = true;
}
