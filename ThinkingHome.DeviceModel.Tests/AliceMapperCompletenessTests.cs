using System.Text.Json.Serialization;
using ThinkingHome.Alice.Mapping;
using ThinkingHome.Alice.Model.Capabilities;
using ThinkingHome.Alice.Model.Capabilities.ColorSetting;
using ThinkingHome.Alice.Model.Capabilities.Mode;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.Alice.Model.Capabilities.Range;
using ThinkingHome.Alice.Model.Capabilities.Toggle;
using ThinkingHome.Alice.Model.Properties;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

/// <summary>
/// Механическая полнота маппера Алисы. Словарь способностей и свойств растёт, а реальных устройств
/// на каждый тип нет — поэтому «забытая ветка» должна ловиться тестом по закрытым иерархиям, а не в
/// проде. Каждый конкретный тип ядра обязан иметь образец в этом файле и маппиться без исключений;
/// каждое действие Алисы — давать нейтральную команду и результат. Новый тип без образца или без
/// ветки маппера → красный тест, называющий тип по имени.
/// </summary>
public class AliceMapperCompletenessTests
{
    // ── образцы: ровно один на конкретный тип (для веток с несколькими instance — по образцу на ветку) ──

    private static readonly Capability[] CapabilitySamples =
    [
        new OnOffCapability { Instance = "on_off" },
        new BrightnessCapability { Instance = "brightness" },
        new ColorCapability { Instance = ColorCapability.InstanceName, Model = ColorModel.Rgb },
        new OpenCapability { Instance = "open" },
        new FanSpeedCapability { Instance = "fan_speed", Speeds = [FanSpeed.Low] },
        new OscillationCapability { Instance = "oscillation" },
        new ThermostatModeCapability { Instance = "thermostat_mode", Modes = [ThermostatMode.Cool] },
        new TargetTemperatureCapability { Instance = "target_temperature", MinCelsius = 18, MaxCelsius = 33 },
    ];

    private static readonly Property[] PropertySamples =
    [
        new TemperatureProperty { Instance = "temperature" },
        new HumidityProperty { Instance = "humidity" },
        new OccupancyProperty { Instance = "occupancy" },
        new ContactProperty { Instance = "contact" },
        new WaterLeakProperty { Instance = "water_leak" },
        new BatteryProperty { Instance = "battery" },
    ];

    private static readonly StateValue[] StateSamples =
    [
        new OnOffState { Instance = "on_off", Value = true },
        new BrightnessState { Instance = "brightness", Value = 50 },
        new ColorRgbState { Instance = ColorCapability.InstanceName, Value = 0xFF0000 },
        new ColorTemperatureState { Instance = ColorCapability.InstanceName, Value = 3500 },
        new OpenState { Instance = "open", Value = 70 },
        new FanSpeedState { Instance = "fan_speed", Value = FanSpeed.Low },
        new OscillationState { Instance = "oscillation", Value = true },
        new ThermostatModeState { Instance = "thermostat_mode", Value = ThermostatMode.Cool },
        new TargetTemperatureState { Instance = "target_temperature", Value = 23 },
        new TemperatureState { Instance = "temperature", Value = 23.5 },
        new HumidityState { Instance = "humidity", Value = 41 },
        new OccupancyState { Instance = "occupancy", Value = true },
        new ContactState { Instance = "contact", Value = true },
        new WaterLeakState { Instance = "water_leak", Value = false },
        new BatteryState { Instance = "battery", Value = 87 },
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
            // значение обязано попасть в одну из веток (capabilities или properties) и не бросить
            Assert.True(state.Capabilities.Length + state.Properties.Length > 0,
                $"Состояние {value.GetType().Name} не дало ни умения, ни свойства Алисы");
        }
    }

    [Fact]
    public void Every_property_type_has_sample_and_maps_to_discovery()
    {
        AssertAllSubtypesCovered(typeof(Property), PropertySamples, "свойства");

        foreach (var property in PropertySamples)
        {
            var device = Assert.Single(AliceMapper.ToDevices(Descriptor(property)));
            Assert.NotEmpty(device.Properties); // ветка ToPropertyInfo существует и не бросает
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

    // осознанное деление слота кэша: альтернативные представления одной способности (прецедент цвета)
    private static readonly Dictionary<string, Type[]> DeliberateSharedSlots = new()
    {
        [ColorCapability.InstanceName] = [typeof(ColorRgbState), typeof(ColorTemperatureState)],
    };

    [Fact]
    public void Neutral_instances_do_not_collide_across_capabilities_and_properties()
    {
        // кэш хоста ключуется (endpoint, instance): способность и свойство с одним instance затирали бы
        // состояния друг друга (прецедент: уставка target_temperature против сенсорной temperature)
        var duplicates = CapabilitySamples.Select(c => c.Instance)
            .Concat(PropertySamples.Select(p => p.Instance))
            .GroupBy(i => i)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToArray();

        Assert.True(duplicates.Length == 0, $"Пересечение канонических instance: {string.Join(", ", duplicates)}");
    }

    [Fact]
    public void State_types_share_instance_slot_only_by_design()
    {
        foreach (var group in StateSamples.GroupBy(s => s.Instance))
        {
            var types = group.Select(s => s.GetType()).Distinct().ToArray();
            if (types.Length == 1)
                continue;

            Assert.True(
                DeliberateSharedSlots.TryGetValue(group.Key, out var allowed) && types.ToHashSet().SetEquals(allowed),
                $"Типы состояний делят слот кэша (endpoint, \"{group.Key}\") вне допуска: {string.Join(", ", types.Select(t => t.Name))}");
        }
    }

    [Fact]
    public void Canonical_instances_are_derived_from_type_names()
    {
        // правило идентификаторов (README ядра): instance не выбирается вручную, а выводится из
        // имени типа — snake_case без суффикса Capability/Property; поэтому пересечения невозможны
        foreach (var (sample, instance) in CapabilitySamples.Select(c => ((object)c, c.Instance))
                     .Concat(PropertySamples.Select(p => ((object)p, p.Instance))))
        {
            Assert.Equal(DerivedInstance(sample.GetType()), instance);
        }
    }

    private static string DerivedInstance(Type type)
    {
        var name = type.Name;
        foreach (var suffix in new[] { "Capability", "Property" })
        {
            if (name.EndsWith(suffix))
            {
                name = name[..^suffix.Length];
                break;
            }
        }

        return string.Concat(name.Select((c, i) => char.IsUpper(c) ? (i > 0 ? "_" : "") + char.ToLowerInvariant(c) : c.ToString()));
    }

    [Fact]
    public void Every_state_instance_is_declared_by_capability_or_property()
    {
        // состояние с instance, которого нет ни у одной способности/свойства, — мёртвый слот кэша
        var declared = CapabilitySamples.Select(c => c.Instance)
            .Concat(PropertySamples.Select(p => p.Instance))
            .ToHashSet();
        var orphans = StateSamples.Select(s => s.Instance).Where(i => !declared.Contains(i)).Distinct().ToArray();

        Assert.True(orphans.Length == 0, $"Состояния с необъявленным instance: {string.Join(", ", orphans)}");
    }

    [Fact]
    public void Discovery_and_state_report_same_alice_property_types_per_instance()
    {
        // как и для умений: объявленное свойство обязано отдаваться в состоянии, и наоборот
        foreach (var property in PropertySamples)
        {
            var declared = Assert.Single(AliceMapper.ToDevices(Descriptor(property)))
                .Properties.Select(p => Discriminator(typeof(PropertyInfoBase), p)).ToHashSet();

            var states = StateSamples.Where(s => s.Instance == property.Instance).ToArray();
            Assert.NotEmpty(states); // у каждого свойства должен быть образец состояния того же instance

            var reported = AliceMapper.ToDeviceState(new AliceDeviceId("d", 0),
                    new DeviceSnapshot { DeviceId = "d", Values = states })
                .Properties.Select(p => Discriminator(typeof(PropertyStateBase), p)).ToHashSet();

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

    private static DeviceDescriptor Descriptor(Property property, DeviceType type = DeviceType.TemperatureSensor) => new()
    {
        Id = "d",
        Title = "T",
        Endpoints = [new Endpoint { Id = 0, Type = type, Properties = [property] }],
    };
}
