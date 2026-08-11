namespace ThinkingHome.DeviceModel.Drivers.Stubs;

/// <summary>Схема секции <c>StubDevices</c> — список виртуальных устройств.</summary>
public sealed class StubsPluginConfig
{
    /// <summary>Устройства, которые нужно создать.</summary>
    public List<StubDeviceEntry> Devices { get; set; } = [];
}

/// <summary>Одно виртуальное устройство в конфигурации.</summary>
public sealed class StubDeviceEntry
{
    /// <summary>Стабильный идентификатор устройства.</summary>
    public string Id { get; set; } = "";

    /// <summary>
    /// Разновидность виртуального устройства — имя значения <see cref="StubDeviceKind"/>.
    /// Строка, а не enum: биндер конфигурации молча выбрасывает записи списка с неконвертируемыми
    /// значениями, а строка биндится всегда — ошибка парсинга остаётся нашей и получает внятный текст.
    /// </summary>
    public string Kind { get; set; } = "";

    /// <summary>Человекочитаемое название.</summary>
    public string Title { get; set; } = "";

    /// <summary>Комната (если есть).</summary>
    public string? Room { get; set; }
}

/// <summary>Разновидности виртуальных устройств.</summary>
public enum StubDeviceKind
{
    /// <summary>Лампа вкл/выкл.</summary>
    OnOffLight,

    /// <summary>Розетка.</summary>
    OnOffSocket,

    /// <summary>Выключатель.</summary>
    OnOffSwitch,

    /// <summary>Лампа с яркостью.</summary>
    DimmableLamp,

    /// <summary>Лампа с цветовой температурой.</summary>
    ColorTemperatureLamp,

    /// <summary>Полноцветная лампа.</summary>
    ColorLamp,

    /// <summary>Штора.</summary>
    Curtain,

    /// <summary>Вентилятор.</summary>
    Fan,

    /// <summary>Кондиционер.</summary>
    AirConditioner,

    /// <summary>Датчик климата: температура, влажность, давление.</summary>
    ClimateSensor,

    /// <summary>Датчик движения.</summary>
    MotionSensor,

    /// <summary>Датчик открытия.</summary>
    ContactSensor,

    /// <summary>Датчик протечки.</summary>
    WaterLeakSensor,

    /// <summary>Датчик освещённости.</summary>
    LightSensor,

    /// <summary>Датчик качества воздуха.</summary>
    AirQualitySensor,

    /// <summary>Двухканальный счётчик воды.</summary>
    WaterMeter,
}
