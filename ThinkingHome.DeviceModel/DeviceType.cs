using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel;

/// <summary>
/// Тип устройства. Значения выровнены на device types из Matter Device Library;
/// в комментарии — идентификатор Matter. Каждый адаптер (Алиса/HomeKit) держит свою
/// таблицу перевода этих значений в свой формат.
///
/// Заведены типы, покрытые текущим словарём способностей и свойств; остальные добавляются со
/// сверкой по Matter.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeviceType
{
    /// <summary>Недиммируемый источник света (только On/Off). Matter 0x0100.</summary>
    OnOffLight,

    /// <summary>Диммируемый источник света (On/Off + яркость). Matter 0x0101.</summary>
    DimmableLight,

    /// <summary>Источник света с регулировкой цветовой температуры. Matter Color Temperature Light 0x010C.</summary>
    ColorTemperatureLight,

    /// <summary>Источник света с полной цветопередачей (RGB + температура). Matter Extended Color Light 0x010D.</summary>
    ExtendedColorLight,

    /// <summary>Управляемая розетка (только On/Off). Matter On/Off Plug-in Unit 0x010A.</summary>
    OnOffSocket,

    /// <summary>Реле/выключатель нагрузки (только On/Off). Matter On/Off Light Switch 0x0103.</summary>
    OnOffSwitch,

    /// <summary>Штора/жалюзи (открытие/закрытие + положение). Matter Window Covering 0x0202.</summary>
    Curtain,

    /// <summary>Вентилятор (On/Off + скорость). Matter Fan 0x002B.</summary>
    Fan,

    /// <summary>Кондиционер (On/Off + уставка температуры + режим + скорость + осцилляция). Matter Room Air Conditioner 0x0072.</summary>
    AirConditioner,

    /// <summary>Датчик температуры (может нести и влажность доп. свойством). Matter Temperature Sensor 0x0302.</summary>
    TemperatureSensor,

    /// <summary>Датчик влажности. Matter Humidity Sensor 0x0307.</summary>
    HumiditySensor,

    /// <summary>Датчик присутствия/движения. Matter Occupancy Sensor 0x0107.</summary>
    OccupancySensor,

    /// <summary>Датчик открытия (контакт). Matter Contact Sensor 0x0015.</summary>
    ContactSensor,
}
