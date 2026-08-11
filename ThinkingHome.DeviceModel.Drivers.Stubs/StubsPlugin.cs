using Microsoft.Extensions.Configuration;

namespace ThinkingHome.DeviceModel.Drivers.Stubs;

/// <summary>
/// Плагин стабовых устройств: читает секцию <c>StubDevices</c> конфигурации и регистрирует
/// виртуальные устройства по списку <see cref="StubsPluginConfig.Devices"/>.
/// </summary>
public sealed class StubsPlugin(IConfiguration configuration) : IDevicePlugin
{
    /// <summary>Имя секции конфигурации, которую читает плагин.</summary>
    public const string SectionName = "StubDevices";

    /// <inheritdoc />
    public void RegisterDevices(IDeviceRegistry registry)
    {
        var options = configuration.GetSection(SectionName).Get<StubsPluginConfig>() ?? new StubsPluginConfig();

        for (var i = 0; i < options.Devices.Count; i++)
        {
            var entry = options.Devices[i];

            if (string.IsNullOrWhiteSpace(entry.Id))
            {
                throw new InvalidOperationException($"{SectionName}:Devices[{i}]: не задан Id.");
            }

            registry.Register(Create(entry));
        }
    }

    private static IDevice Create(StubDeviceEntry entry)
    {
        var config = new StubDeviceConfig { Title = entry.Title, Room = entry.Room };

        return entry.Kind switch
        {
            StubDeviceKind.OnOffLight => OnOff(entry, DeviceType.OnOffLight),
            StubDeviceKind.OnOffSocket => OnOff(entry, DeviceType.OnOffSocket),
            StubDeviceKind.OnOffSwitch => OnOff(entry, DeviceType.OnOffSwitch),
            StubDeviceKind.DimmableLamp => new StubDimmableLamp(entry.Id, config),
            StubDeviceKind.ColorTemperatureLamp => new StubColorTemperatureLamp(entry.Id, config),
            StubDeviceKind.ColorLamp => new StubColorLamp(entry.Id, config),
            StubDeviceKind.Curtain => new StubCurtain(entry.Id, config),
            StubDeviceKind.Fan => new StubFan(entry.Id, config),
            StubDeviceKind.AirConditioner => new StubAirConditioner(entry.Id, config),
            StubDeviceKind.ClimateSensor => new StubClimateSensor(entry.Id, config),
            StubDeviceKind.MotionSensor => new StubMotionSensor(entry.Id, config),
            StubDeviceKind.ContactSensor => new StubContactSensor(entry.Id, config),
            StubDeviceKind.WaterLeakSensor => new StubWaterLeakSensor(entry.Id, config),
            StubDeviceKind.LightSensor => new StubLightSensor(entry.Id, config),
            StubDeviceKind.AirQualitySensor => new StubAirQualitySensor(entry.Id, config),
            StubDeviceKind.WaterMeter => new StubWaterius(entry.Id, config),
            _ => throw new InvalidOperationException(
                $"{SectionName}: устройство '{entry.Id}' — неизвестный Kind '{entry.Kind}'."),
        };
    }

    private static StubOnOffDevice OnOff(StubDeviceEntry entry, DeviceType type)
        => new(entry.Id, new StubOnOffDeviceConfig { Title = entry.Title, Room = entry.Room, Type = type });
}
