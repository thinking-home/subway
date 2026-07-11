namespace ThinkingHome.DeviceModel.State;

/// <summary>Снимок состояния устройства — ответ на Query.</summary>
public sealed record DeviceSnapshot
{
    public required string DeviceId { get; init; }
    public required IReadOnlyList<StateValue> Values { get; init; }
}
