namespace ThinkingHome.DeviceModel.State;

/// <summary>Снимок состояния устройства — ответ на Query.</summary>
public sealed record DeviceSnapshot
{
    /// <summary>Устройство, чьё это состояние.</summary>
    public required string DeviceId { get; init; }
    /// <summary>Все текущие значения способностей и свойств устройства.</summary>
    public required IReadOnlyList<StateValue> Values { get; init; }
}
