namespace ThinkingHome.DeviceModel;

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

/// <summary>Снимок состояния устройства — ответ на Query.</summary>
public sealed record DeviceSnapshot
{
    public required string DeviceId { get; init; }
    public required IReadOnlyList<StateValue> Values { get; init; }
}

/// <summary>Отчёт об изменении состояния — событие Report (push).</summary>
public sealed record StateChange
{
    public required string DeviceId { get; init; }
    public required StateValue Value { get; init; }
}
