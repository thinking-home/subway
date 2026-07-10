namespace ThinkingHome.DeviceModel;

/// <summary>
/// Тип устройства. Значения выровнены на device types из Matter Device Library;
/// в комментарии — идентификатор Matter. Каждый адаптер (Алиса/HomeKit) держит свою
/// таблицу перевода этих значений в свой формат.
///
/// Пока заведён только тип для первого сквозного примера; остальные добавляются со сверкой по Matter.
/// </summary>
public enum DeviceType
{
    /// <summary>Недиммируемый источник света (только On/Off). Matter 0x0100.</summary>
    OnOffLight,
}
