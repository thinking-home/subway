namespace ThinkingHome.DeviceModel.State;

/// <summary>Отчёт об изменении состояния — событие Report (push).</summary>
public sealed record StateChange
{
    /// <summary>Устройство, чьё состояние изменилось.</summary>
    public required string DeviceId { get; init; }
    /// <summary>Новое значение (endpoint и instance — внутри значения).</summary>
    public required StateValue Value { get; init; }
}
