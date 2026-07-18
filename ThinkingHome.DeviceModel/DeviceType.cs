using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel;

/// <summary>
/// Тип устройства. Значения выровнены на device types из Matter Device Library;
/// в комментарии — идентификатор Matter. Каждый адаптер (Алиса/HomeKit) держит свою
/// таблицу перевода этих значений в свой формат.
///
/// Заведены типы, покрытые способностью On/Off; остальные добавляются со сверкой по Matter.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeviceType
{
    /// <summary>Недиммируемый источник света (только On/Off). Matter 0x0100.</summary>
    OnOffLight,

    /// <summary>Диммируемый источник света (On/Off + яркость). Matter 0x0101.</summary>
    DimmableLight,

    /// <summary>Управляемая розетка (только On/Off). Matter On/Off Plug-in Unit 0x010A.</summary>
    OnOffSocket,

    /// <summary>Реле/выключатель нагрузки (только On/Off). Matter On/Off Light Switch 0x0103.</summary>
    OnOffSwitch,
}
