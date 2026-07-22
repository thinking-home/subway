using System.Text.Json.Serialization;
using ThinkingHome.Alice.Mapping;
using ThinkingHome.Alice.Model.Capabilities;
using ThinkingHome.Alice.Model.Capabilities.ColorSetting;
using ThinkingHome.Alice.Model.Capabilities.Mode;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.Alice.Model.Capabilities.Range;
using ThinkingHome.Alice.Model.Capabilities.Toggle;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

/// <summary>
/// Механическая полнота маппера Алисы. Словарь способностей растёт, а реальных устройств на каждый
/// тип нет — поэтому «забытая ветка» должна ловиться тестом по закрытым иерархиям, а не в проде.
/// Каждый конкретный тип ядра обязан иметь образец в этом файле и маппиться без исключений; каждое
/// действие Алисы — давать нейтральную команду и результат. Новый тип без образца или без ветки
/// маппера → красный тест, называющий тип по имени.
/// </summary>
public class AliceMapperCompletenessTests
{
    // ── образцы: ровно один на конкретный тип (для веток с несколькими instance — по образцу на ветку) ──

    private static readonly Capability[] CapabilitySamples =
    [
        new OnOffCapability { Instance = "on" },
        new BrightnessCapability { Instance = "brightness" },
        new ColorCapability { Instance = ColorCapability.InstanceName, Model = ColorModel.Rgb },
        new OpenCapability { Instance = "open" },
        new FanSpeedCapability { Instance = "fan_speed", Speeds = [FanSpeed.Low] },
        new OscillationCapability { Instance = "oscillation" },
        new ThermostatModeCapability { Instance = "thermostat", Modes = [ThermostatMode.Cool] },
        new TargetTemperatureCapability { Instance = "temperature", MinCelsius = 18, MaxCelsius = 33 },
    ];

    private static readonly StateValue[] StateSamples =
    [
        new OnOffState { Instance = "on", Value = true },
        new BrightnessState { Instance = "brightness", Value = 50 },
        new ColorRgbState { Instance = ColorCapability.InstanceName, Value = 0xFF0000 },
        new ColorTemperatureState { Instance = ColorCapability.InstanceName, Value = 3500 },
        new OpenState { Instance = "open", Value = 70 },
        new FanSpeedState { Instance = "fan_speed", Value = FanSpeed.Low },
        new OscillationState { Instance = "oscillation", Value = true },
        new ThermostatModeState { Instance = "thermostat", Value = ThermostatMode.Cool },
        new TargetTemperatureState { Instance = "temperature", Value = 23 },
    ];

    // все поддерживаемые действия Алисы — по одному образцу на каждую ветку ToCommand (тип + instance)
    private static readonly CapabilityActionParamsBase[] ActionSamples =
    [
        new CapabilityActionParamsOnOff { State = new CapabilityStateOnOffData { Instance = CapabilityStateOnOffInstance.On, Value = true } },
        new CapabilityActionParamsRange { State = new CapabilityStateRangeData { Instance = CapabilityStateRangeInstance.Brightness, Value = 50 } },
        new CapabilityActionParamsRange { State = new CapabilityStateRangeData { Instance = CapabilityStateRangeInstance.Open, Value = 70 } },
        new CapabilityActionParamsRange { State = new CapabilityStateRangeData { Instance = CapabilityStateRangeInstance.Temperature, Value = 23 } },
        new CapabilityActionParamsColorSetting { State = new CapabilityStateColorData { Instance = CapabilityColorInstance.Rgb, Value = 0xFF0000 } },
        new CapabilityActionParamsColorSetting { State = new CapabilityStateColorData { Instance = CapabilityColorInstance.TemperatureK, Value = 3500 } },
        new CapabilityActionParamsMode { State = new CapabilityStateModeData { Instance = CapabilityModeInstance.FanSpeed, Value = CapabilityModeValue.Low } },
        new CapabilityActionParamsMode { State = new CapabilityStateModeData { Instance = CapabilityModeInstance.Thermostat, Value = CapabilityModeValue.Cool } },
        new CapabilityActionParamsToggle { State = new CapabilityStateToggleData { Instance = CapabilityToggleInstance.Oscillation, Value = true } },
    ];

    [Fact]
    public void Every_capability_type_has_sample_and_maps_to_discovery()
    {
        AssertAllSubtypesCovered(typeof(Capability), CapabilitySamples, "способности");

        foreach (var capability in CapabilitySamples)
        {
            var device = Assert.Single(AliceMapper.ToDevices(Descriptor(capability)));
            Assert.NotEmpty(device.Capabilities); // ветка ToCapabilityInfo существует и не бросает
        }
    }

    [Fact]
    public void Every_state_type_has_sample_and_maps_to_alice_state()
    {
        AssertAllSubtypesCovered(typeof(StateValue), StateSamples, "состояния");

        foreach (var value in StateSamples)
        {
            var state = AliceMapper.ToDeviceState(new AliceDeviceId("d", 0),
                new DeviceSnapshot { DeviceId = "d", Values = [value] });
            Assert.NotEmpty(state.Capabilities); // ветка ToCapabilityState существует и не бросает
        }
    }

    [Fact]
    public void Every_alice_action_type_has_sample_and_maps_to_command_and_result()
    {
        // полнота — по [JsonDerivedType] на базе действий: что Алиса может прислать, то мы обязаны понять
        var covered = ActionSamples.Select(s => s.GetType()).ToHashSet();
        var missing = RegisteredDerivedTypes(typeof(CapabilityActionParamsBase))
            .Where(t => !covered.Contains(t))
            .Select(t => t.Name)
            .ToArray();
        Assert.True(missing.Length == 0, $"Нет образца действия Алисы: {string.Join(", ", missing)}");

        foreach (var action in ActionSamples)
        {
            Assert.NotNull(AliceMapper.ToCommand(action, endpointId: 0));
            Assert.NotNull(AliceMapper.ToCapabilityActionResult(action, CommandOutcome.Done));
        }
    }

    [Fact]
    public void Every_neutral_command_type_is_reachable_from_alice_actions()
    {
        // мёртвая команда (недостижимая из действий Алисы) — признак забытой ветки ToCommand
        var reachable = ActionSamples.Select(a => AliceMapper.ToCommand(a, 0).GetType()).ToHashSet();
        var unreachable = ConcreteSubtypes(typeof(DeviceCommand))
            .Where(t => !reachable.Contains(t))
            .Select(t => t.Name)
            .ToArray();
        Assert.True(unreachable.Length == 0, $"Команды недостижимы из действий Алисы: {string.Join(", ", unreachable)}");
    }

    [Fact]
    public void Every_device_type_maps_to_alice()
    {
        foreach (var type in Enum.GetValues<DeviceType>())
            Assert.Single(AliceMapper.ToDevices(Descriptor(CapabilitySamples[0], type))); // ветка ToAliceDeviceType есть
    }

    [Fact]
    public void Discovery_and_state_report_same_alice_capability_types_per_instance()
    {
        // Алиса игнорирует состояние необъявленного умения (и наоборот): для каждого instance ядра
        // набор типов умений в discovery обязан совпадать с набором типов в состоянии — включая derivation.
        foreach (var capability in CapabilitySamples)
        {
            var declared = Assert.Single(AliceMapper.ToDevices(Descriptor(capability)))
                .Capabilities.Select(c => Discriminator(typeof(CapabilityInfoBase), c)).ToHashSet();

            var states = StateSamples.Where(s => s.Instance == capability.Instance).ToArray();
            Assert.NotEmpty(states); // у каждой способности должен быть образец состояния того же instance

            var reported = AliceMapper.ToDeviceState(new AliceDeviceId("d", 0),
                    new DeviceSnapshot { DeviceId = "d", Values = states })
                .Capabilities.Select(c => Discriminator(typeof(CapabilityStateBase), c)).ToHashSet();

            Assert.Equal(declared, reported);
        }
    }

    // ── вспомогательное ──

    private static IEnumerable<Type> ConcreteSubtypes(Type baseType) =>
        baseType.Assembly.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t));

    private static IEnumerable<Type> RegisteredDerivedTypes(Type baseType) =>
        baseType.GetCustomAttributes(typeof(JsonDerivedTypeAttribute), false)
            .Cast<JsonDerivedTypeAttribute>()
            .Select(a => a.DerivedType);

    private static void AssertAllSubtypesCovered<T>(Type baseType, T[] samples, string kind) where T : notnull
    {
        var covered = samples.Select(s => s.GetType()).ToHashSet();
        var missing = ConcreteSubtypes(baseType).Where(t => !covered.Contains(t)).Select(t => t.Name).ToArray();
        Assert.True(missing.Length == 0, $"Нет образца {kind}: {string.Join(", ", missing)}");
    }

    // тип умения Алисы ("devices.capabilities.*") конкретного DTO — по его регистрации в [JsonDerivedType]
    private static string Discriminator(Type baseType, object dto) =>
        baseType.GetCustomAttributes(typeof(JsonDerivedTypeAttribute), false)
            .Cast<JsonDerivedTypeAttribute>()
            .Single(a => a.DerivedType == dto.GetType())
            .TypeDiscriminator!.ToString()!;

    private static DeviceDescriptor Descriptor(Capability capability, DeviceType type = DeviceType.Fan) => new()
    {
        Id = "d",
        Title = "T",
        Endpoints = [new Endpoint { Id = 0, Type = type, Capabilities = [capability] }],
    };
}
