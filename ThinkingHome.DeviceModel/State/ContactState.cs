namespace ThinkingHome.DeviceModel.State;

/// <summary>Текущее состояние контакта датчика открытия (instance "contact"). Семантика Matter: true — контакт замкнут (закрыто).</summary>
public sealed record ContactState : StateValue
{
    public required bool Value { get; init; }
}
