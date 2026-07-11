namespace ThinkingHome.DeviceModel.State;

/// <summary>
/// Текущее значение способности или свойства. Закрытая иерархия, параллельная командам и описаниям.
/// </summary>
public abstract record StateValue
{
    /// <summary>Endpoint устройства (0 — основной).</summary>
    public int EndpointId { get; init; }

    /// <summary>Экземпляр способности/свойства, к которому относится значение.</summary>
    public required string Instance { get; init; }
}
