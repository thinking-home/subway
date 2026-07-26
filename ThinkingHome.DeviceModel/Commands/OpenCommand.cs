namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить степень открытия 0–100 % (instance "open").</summary>
public sealed record OpenCommand : DeviceCommand
{
    /// <summary>Целевая степень открытия, % (0 — закрыто, 100 — открыто).</summary>
    public required int Value { get; init; }
}
