using System.Text.Json;
using System.Text.Json.Serialization;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

public class SerializationTests
{
    public static IEnumerable<object[]> PolymorphicBases => new[]
    {
        new object[] { typeof(Capability) },
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

    [Fact]
    public void Command_round_trips_polymorphically()
    {
        DeviceCommand command = new OnOffCommand { Instance = "on", Value = true };

        var json = JsonSerializer.Serialize(command);
        var back = JsonSerializer.Deserialize<DeviceCommand>(json);

        var onOff = Assert.IsType<OnOffCommand>(back);
        Assert.True(onOff.Value);
        Assert.Equal("on", onOff.Instance);
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
        DeviceCommand temp = new ColorTemperatureCommand { Instance = "temperature_k", Value = 3500 };
        DeviceCommand rgb = new ColorCommand { Instance = "rgb", Value = 0x00FF00 };

        Assert.Equal(3500, Assert.IsType<ColorTemperatureCommand>(
            JsonSerializer.Deserialize<DeviceCommand>(JsonSerializer.Serialize(temp))).Value);
        Assert.Equal(0x00FF00, Assert.IsType<ColorCommand>(
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
    public void Snapshot_round_trips_with_polymorphic_values()
    {
        var snapshot = new DeviceSnapshot
        {
            DeviceId = "lamp",
            Values = [new OnOffState { Instance = "on", Value = true }],
        };

        var json = JsonSerializer.Serialize(snapshot);
        var back = JsonSerializer.Deserialize<DeviceSnapshot>(json)!;

        var state = Assert.IsType<OnOffState>(Assert.Single(back.Values));
        Assert.True(state.Value);
    }
}
