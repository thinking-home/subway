using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.Home;

/// <summary>
/// Заглушка лампы с полной цветопередачей (OnOff + яркость + RGB + цветовая температура). Держит один
/// активный цветовой режим и отдаёт в снимке именно его — у color_setting остаётся одно состояние.
/// </summary>
public sealed class StubColorLamp(string id, string title, string? room = null) : IDevice
{
    private bool isOn;
    private int brightness = 100;
    private int kelvin = 4000;
    private int rgb = 0xFFFFFF;
    private bool rgbMode; // false — температура, true — rgb

    public string Id => id;

    public event Action<StateChange>? Changed;

    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Room = room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub-rgb" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = DeviceType.ExtendedColorLight,
            Capabilities =
            [
                new OnOffCapability { Instance = "on_off" },
                new BrightnessCapability { Instance = "brightness" },
                new ColorCapability
                {
                    Instance = ColorCapability.InstanceName,
                    Model = ColorModel.Rgb,
                    Temperature = new ColorTemperatureRange { MinKelvin = 2700, MaxKelvin = 6500 },
                },
            ],
        }],
    };

    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values =
            [
                new OnOffState { Instance = "on_off", Value = isOn },
                new BrightnessState { Instance = "brightness", Value = brightness },
                rgbMode
                    ? new ColorRgbState { Instance = ColorCapability.InstanceName, Value = rgb }
                    : new ColorTemperatureState { Instance = ColorCapability.InstanceName, Value = kelvin },
            ],
        });

    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
    {
        switch (command)
        {
            case OnOffCommand on:
                isOn = on.Value;
                Console.WriteLine($"[{id}] → {(isOn ? "ВКЛ" : "выкл")}");
                Report(new OnOffState { Instance = "on_off", Value = isOn });
                return Task.FromResult(CommandOutcome.Done);

            case BrightnessCommand br:
                brightness = br.Value;
                Console.WriteLine($"[{id}] → яркость {brightness}%");
                Report(new BrightnessState { Instance = "brightness", Value = brightness });
                return Task.FromResult(CommandOutcome.Done);

            case ColorTemperatureCommand temp:
                kelvin = temp.Value;
                rgbMode = false;
                Console.WriteLine($"[{id}] → температура {kelvin}K");
                Report(new ColorTemperatureState { Instance = ColorCapability.InstanceName, Value = kelvin });
                return Task.FromResult(CommandOutcome.Done);

            case ColorRgbCommand color:
                rgb = color.Value;
                rgbMode = true;
                Console.WriteLine($"[{id}] → цвет #{rgb:X6}");
                Report(new ColorRgbState { Instance = ColorCapability.InstanceName, Value = rgb });
                return Task.FromResult(CommandOutcome.Done);

            default:
                return Task.FromResult(CommandOutcome.Unsupported);
        }
    }

    private void Report(StateValue value) => Changed?.Invoke(new StateChange { DeviceId = id, Value = value });
}
