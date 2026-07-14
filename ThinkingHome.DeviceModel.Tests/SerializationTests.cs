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
