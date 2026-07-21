namespace ThinkingHome.DeviceModel.Commands;

/// <summary>Установить цвет RGB, 0xRRGGBB (instance "rgb").</summary>
public sealed record ColorCommand : DeviceCommand
{
    public required int Value { get; init; }
}
