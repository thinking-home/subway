namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить степень открытия 0–100 % (instance "open").</summary>
public sealed record OpenCommand : DeviceCommand
{
    public required int Value { get; init; }
}
