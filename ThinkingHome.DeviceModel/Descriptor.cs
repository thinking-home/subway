namespace ThinkingHome.DeviceModel;

/// <summary>Полное описание устройства — то, что драйвер отдаёт при discovery.</summary>
public sealed record DeviceDescriptor
{
    /// <summary>Стабильный идентификатор, переживающий рестарты.</summary>
    public required string Id { get; init; }

    public required string Title { get; init; }

    public string? Room { get; init; }

    public DeviceManufacturer? Manufacturer { get; init; }

    /// <summary>Endpoint'ы устройства. Простое устройство имеет один endpoint (Id = 0).</summary>
    public required IReadOnlyList<Endpoint> Endpoints { get; init; }
}

/// <summary>Логическая часть устройства: тип + способности и свойства.</summary>
public sealed record Endpoint
{
    /// <summary>Номер endpoint'а внутри устройства (0 — основной).</summary>
    public required int Id { get; init; }

    public required DeviceType Type { get; init; }

    /// <summary>Способности — то, чем можно управлять (актуаторы).</summary>
    public IReadOnlyList<Capability> Capabilities { get; init; } = [];

    /// <summary>Свойства — то, что можно только читать (сенсоры/события).</summary>
    public IReadOnlyList<Property> Properties { get; init; } = [];
}

public sealed record DeviceManufacturer
{
    public string? Name { get; init; }
    public string? Model { get; init; }
    public string? HardwareVersion { get; init; }
    public string? SoftwareVersion { get; init; }
}
