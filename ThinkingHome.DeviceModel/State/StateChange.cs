namespace ThinkingHome.DeviceModel.State;

/// <summary>Отчёт об изменении состояния — событие Report (push).</summary>
public sealed record StateChange
{
    public required string DeviceId { get; init; }
    public required StateValue Value { get; init; }
}
