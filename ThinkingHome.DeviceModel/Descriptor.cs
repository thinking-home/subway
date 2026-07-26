using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Properties;

namespace ThinkingHome.DeviceModel;

/// <summary>Полное описание устройства — то, что драйвер отдаёт при discovery.</summary>
public sealed record DeviceDescriptor
{
    /// <summary>Стабильный идентификатор, переживающий рестарты.</summary>
    public required string Id { get; init; }

    /// <summary>Человекочитаемое название устройства.</summary>
    public required string Title { get; init; }

    /// <summary>Комната, где установлено устройство (если известна).</summary>
    public string? Room { get; init; }

    /// <summary>Паспортные данные устройства (производитель, модель, версии).</summary>
    public DeviceManufacturer? Manufacturer { get; init; }

    /// <summary>Endpoint'ы устройства. Простое устройство имеет один endpoint (Id = 0).</summary>
    public required IReadOnlyList<Endpoint> Endpoints { get; init; }
}

/// <summary>Логическая часть устройства: тип + способности и свойства.</summary>
public sealed record Endpoint
{
    /// <summary>Номер endpoint'а внутри устройства (0 — основной).</summary>
    public required int Id { get; init; }

    /// <summary>Тип endpoint'а — роль из каталога Matter.</summary>
    public required DeviceType Type { get; init; }

    /// <summary>Способности — то, чем можно управлять (актуаторы).</summary>
    public IReadOnlyList<Capability> Capabilities { get; init; } = [];

    /// <summary>Свойства — то, что можно только читать (сенсоры/события).</summary>
    public IReadOnlyList<Property> Properties { get; init; } = [];
}

/// <summary>Паспортные данные устройства: производитель, модель, версии.</summary>
public sealed record DeviceManufacturer
{
    /// <summary>Название производителя.</summary>
    public string? Name { get; init; }
    /// <summary>Модель устройства.</summary>
    public string? Model { get; init; }
    /// <summary>Версия аппаратной ревизии.</summary>
    public string? HardwareVersion { get; init; }
    /// <summary>Версия прошивки/ПО.</summary>
    public string? SoftwareVersion { get; init; }
}
