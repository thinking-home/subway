namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущее состояние контакта датчика открытия (instance "contact"). Семантика Matter: true — контакт замкнут (закрыто).</summary>
public sealed record ContactState : StateValue
{
    /// <summary>Контакт замкнут (семантика Matter: true — закрыто).</summary>
    public required bool Value { get; init; }
}
