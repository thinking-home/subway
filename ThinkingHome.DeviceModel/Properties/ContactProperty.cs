namespace ThinkingHome.DeviceModel.Properties;

/// <summary>Контакт датчика открытия (bool, семантика Matter: true — контакт замкнут, т.е. закрыто). Matter cluster Boolean State (0x0045). Instance — "contact".</summary>
public sealed record ContactProperty : Property;
