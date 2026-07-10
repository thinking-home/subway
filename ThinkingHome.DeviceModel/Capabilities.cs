namespace ThinkingHome.DeviceModel;

/// <summary>
/// Способность устройства (актуатор). Закрытая иерархия: конкретный тип служит дискриминатором
/// вместо строки. Новая способность добавляется осознанно — словарь выровнен на кластеры Matter.
/// </summary>
public abstract record Capability
{
    /// <summary>Экземпляр способности (например, "on", "brightness").</summary>
    public required string Instance { get; init; }

    /// <summary>Можно ли опросить текущее состояние.</summary>
    public bool Retrievable { get; init; } = true;

    /// <summary>Присылает ли устройство изменения состояния (push).</summary>
    public bool Reportable { get; init; } = true;
}
