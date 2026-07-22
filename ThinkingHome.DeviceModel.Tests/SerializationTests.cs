using System.Text.Json;
using System.Text.Json.Serialization;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

public class SerializationTests
{
    public static IEnumerable<object[]> PolymorphicBases => new[]
    {
        new object[] { typeof(Capability) },
        new object[] { typeof(Property) },
        new object[] { typeof(DeviceCommand) },
        new object[] { typeof(StateValue) },
    };

    [Theory]
    [MemberData(nameof(PolymorphicBases))]
    public void Every_concrete_subtype_is_registered_for_json(Type baseType)
    {
        var registered = baseType.GetCustomAttributes(typeof(JsonDerivedTypeAttribute), false)
            .Cast<JsonDerivedTypeAttribute>()
            .Select(a => a.DerivedType)
            .ToHashSet();

        var missing = baseType.Assembly.GetTypes()
            .Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t))
            .Where(t => !registered.Contains(t))
            .Select(t => t.Name)
            .ToArray();

        Assert.True(missing.Length == 0, $"Не зарегистрированы в [JsonDerivedType]: {string.Join(", ", missing)}");
    }

    [Theory]
    [MemberData(nameof(PolymorphicBases))]
    public void Json_discriminators_are_derived_from_type_names(Type baseType)
    {
        // правило идентификаторов (README ядра): $type = camelCase имени типа без суффикса
        foreach (var attr in baseType.GetCustomAttributes(typeof(JsonDerivedTypeAttribute), false)
                     .Cast<JsonDerivedTypeAttribute>())
        {
            var name = attr.DerivedType.Name;
            foreach (var suffix in new[] { "Capability", "Property", "Command", "State" })
            {
                if (name.EndsWith(suffix))
                {
                    name = name[..^suffix.Length];
                    break;
                }
            }

            Assert.Equal(char.ToLowerInvariant(name[0]) + name[1..], attr.TypeDiscriminator as string);
        }
    }

    [Fact]
    public void Command_round_trips_polymorphically()
    {
        DeviceCommand command = new OnOffCommand { Instance = "on_off", Value = true };

        var json = JsonSerializer.Serialize(command);
        var back = JsonSerializer.Deserialize<DeviceCommand>(json);

        var onOff = Assert.IsType<OnOffCommand>(back);
        Assert.True(onOff.Value);
        Assert.Equal("on_off", onOff.Instance);
    }

    [Fact]
    public void Brightness_command_round_trips_polymorphically()
    {
        DeviceCommand command = new BrightnessCommand { Instance = "brightness", Value = 42 };

        var json = JsonSerializer.Serialize(command);
        var back = JsonSerializer.Deserialize<DeviceCommand>(json);

        var brightness = Assert.IsType<BrightnessCommand>(back);
        Assert.Equal(42, brightness.Value);
        Assert.Equal("brightness", brightness.Instance);
    }

    [Fact]
    public void Color_commands_round_trip_polymorphically()
    {
        DeviceCommand temp = new ColorTemperatureCommand { Instance = ColorCapability.InstanceName, Value = 3500 };
        DeviceCommand rgb = new ColorRgbCommand { Instance = ColorCapability.InstanceName, Value = 0x00FF00 };

        Assert.Equal(3500, Assert.IsType<ColorTemperatureCommand>(
            JsonSerializer.Deserialize<DeviceCommand>(JsonSerializer.Serialize(temp))).Value);
        Assert.Equal(0x00FF00, Assert.IsType<ColorRgbCommand>(
            JsonSerializer.Deserialize<DeviceCommand>(JsonSerializer.Serialize(rgb))).Value);
    }

    [Fact]
    public void Open_command_round_trips_polymorphically()
    {
        DeviceCommand command = new OpenCommand { Instance = "open", Value = 70 };

        var open = Assert.IsType<OpenCommand>(JsonSerializer.Deserialize<DeviceCommand>(JsonSerializer.Serialize(command)));
        Assert.Equal(70, open.Value);
        Assert.Equal("open", open.Instance);
    }

    [Fact]
    public void FanSpeed_command_round_trips_polymorphically()
    {
        DeviceCommand command = new FanSpeedCommand { Instance = "fan_speed", Value = FanSpeed.Medium };

        var back = Assert.IsType<FanSpeedCommand>(JsonSerializer.Deserialize<DeviceCommand>(JsonSerializer.Serialize(command)));
        Assert.Equal(FanSpeed.Medium, back.Value);
    }

    [Fact]
    public void Oscillation_command_round_trips_polymorphically()
    {
        DeviceCommand command = new OscillationCommand { Instance = "oscillation", Value = true };

        var back = Assert.IsType<OscillationCommand>(JsonSerializer.Deserialize<DeviceCommand>(JsonSerializer.Serialize(command)));
        Assert.True(back.Value);
        Assert.Equal("oscillation", back.Instance);
    }

    [Fact]
    public void Thermostat_commands_round_trip_polymorphically()
    {
        DeviceCommand mode = new ThermostatModeCommand { Instance = "thermostat_mode", Value = ThermostatMode.Dry };
        DeviceCommand temp = new TargetTemperatureCommand { Instance = "target_temperature", Value = 24 };

        Assert.Equal(ThermostatMode.Dry, Assert.IsType<ThermostatModeCommand>(
            JsonSerializer.Deserialize<DeviceCommand>(JsonSerializer.Serialize(mode))).Value);
        Assert.Equal(24, Assert.IsType<TargetTemperatureCommand>(
            JsonSerializer.Deserialize<DeviceCommand>(JsonSerializer.Serialize(temp))).Value);
    }

    [Fact]
    public void Sensor_states_round_trip_polymorphically()
    {
        StateValue temperature = new TemperatureState { Instance = "temperature", Value = 23.5 };
        StateValue contact = new ContactState { Instance = "contact", Value = true };

        Assert.Equal(23.5, Assert.IsType<TemperatureState>(
            JsonSerializer.Deserialize<StateValue>(JsonSerializer.Serialize(temperature))).Value);
        Assert.True(Assert.IsType<ContactState>(
            JsonSerializer.Deserialize<StateValue>(JsonSerializer.Serialize(contact))).Value);
    }

    [Fact]
    public void Property_round_trips_polymorphically_in_descriptor()
    {
        var endpoint = new Endpoint
        {
            Id = 0,
            Type = DeviceType.TemperatureSensor,
            Properties = [new TemperatureProperty { Instance = "temperature" }, new HumidityProperty { Instance = "humidity" }],
        };

        var json = JsonSerializer.Serialize(endpoint);
        var back = JsonSerializer.Deserialize<Endpoint>(json)!;

        Assert.Collection(back.Properties,
            p => Assert.IsType<TemperatureProperty>(p),
            p => Assert.IsType<HumidityProperty>(p));
    }

    [Fact]
    public void Snapshot_round_trips_with_polymorphic_values()
    {
        var snapshot = new DeviceSnapshot
        {
            DeviceId = "lamp",
            Values = [new OnOffState { Instance = "on_off", Value = true }],
        };

        var json = JsonSerializer.Serialize(snapshot);
        var back = JsonSerializer.Deserialize<DeviceSnapshot>(json)!;

        var state = Assert.IsType<OnOffState>(Assert.Single(back.Values));
        Assert.True(state.Value);
    }
}
