namespace ThinkingHome.DeviceModel;

/// <summary>
/// Команда изменения состояния способности. Закрытая иерархия: тип-дискриминатор + родное значение.
/// Драйвер разбирает команду через pattern matching (switch по типу).
/// </summary>
public abstract record DeviceCommand
{
    /// <summary>Endpoint устройства (0 — основной).</summary>
    public int EndpointId { get; init; }

    /// <summary>Экземпляр способности, к которой относится команда.</summary>
    public required string Instance { get; init; }
}
