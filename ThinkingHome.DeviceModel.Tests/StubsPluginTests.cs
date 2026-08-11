using Microsoft.Extensions.Configuration;
using ThinkingHome.DeviceModel.Drivers.Stubs;

namespace ThinkingHome.DeviceModel.Tests;

public class StubsPluginTests
{
    [Fact]
    public async Task Plugin_creates_devices_from_its_section()
    {
        var host = new DeviceHost();
        var plugin = new StubsPlugin(Config(
            ("StubDevices:Devices:0:Id", "socket-1"),
            ("StubDevices:Devices:0:Kind", "OnOffSocket"),
            ("StubDevices:Devices:0:Title", "Розетка у стола"),
            ("StubDevices:Devices:0:Room", "Кабинет"),
            ("StubDevices:Devices:1:Id", "water-1"),
            ("StubDevices:Devices:1:Kind", "WaterMeter"),
            ("StubDevices:Devices:1:Title", "Счётчики воды")));

        plugin.RegisterDevices(host);

        Assert.Equal(2, host.Count);

        var socket = await host.GetDeviceAsync("socket-1");
        Assert.NotNull(socket);
        Assert.Equal("Розетка у стола", socket.Title);
        Assert.Equal("Кабинет", socket.Room);
        Assert.Equal(DeviceType.OnOffSocket, Assert.Single(socket.Endpoints).Type);

        var water = await host.GetDeviceAsync("water-1");
        Assert.NotNull(water);
        Assert.Equal(2, water.Endpoints.Count); // Waterius — двухканальный: два endpoint'а
    }

    [Fact]
    public void Every_kind_is_creatable() // защита от забытой ветки switch при добавлении Kind
    {
        var kinds = Enum.GetValues<StubDeviceKind>();
        var values = kinds.SelectMany((kind, i) => new (string, string?)[]
        {
            ($"StubDevices:Devices:{i}:Id", $"dev-{i}"),
            ($"StubDevices:Devices:{i}:Kind", kind.ToString()),
            ($"StubDevices:Devices:{i}:Title", kind.ToString()),
        }).ToArray();

        var host = new DeviceHost();
        new StubsPlugin(Config(values)).RegisterDevices(host);

        Assert.Equal(kinds.Length, host.Count);
    }

    [Fact]
    public void Empty_id_fails_with_position()
    {
        var plugin = new StubsPlugin(Config(
            ("StubDevices:Devices:0:Id", "ok-1"),
            ("StubDevices:Devices:0:Kind", "OnOffLight"),
            ("StubDevices:Devices:1:Id", ""),
            ("StubDevices:Devices:1:Kind", "OnOffLight")));

        var ex = Assert.Throws<InvalidOperationException>(() => plugin.RegisterDevices(new DeviceHost()));
        Assert.Contains("Devices[1]", ex.Message);
    }

    [Theory]
    [InlineData("Teleport")] // опечатка
    [InlineData("42")]       // TryParse "успешно" парсит число в несуществующее значение
    public void Unknown_kind_fails_with_device_id_and_allowed_values(string kind)
    {
        // Kind в схеме — строка, а не enum: биндер молча выбрасывает записи списка
        // с неконвертируемыми значениями, а строку парсим сами с внятной ошибкой
        var plugin = new StubsPlugin(Config(
            ("StubDevices:Devices:0:Id", "dev-1"),
            ("StubDevices:Devices:0:Kind", kind)));

        var ex = Assert.Throws<InvalidOperationException>(() => plugin.RegisterDevices(new DeviceHost()));
        Assert.Contains("dev-1", ex.Message);
        Assert.Contains(kind, ex.Message);
        Assert.Contains(nameof(StubDeviceKind.DimmableLamp), ex.Message); // список допустимых на месте
    }

    [Fact]
    public void Missing_section_registers_nothing()
    {
        var host = new DeviceHost();
        new StubsPlugin(Config()).RegisterDevices(host);

        Assert.Equal(0, host.Count);
    }

    private static IConfiguration Config(params (string Key, string? Value)[] values)
        => new ConfigurationBuilder()
            .AddInMemoryCollection(values.ToDictionary(v => v.Key, v => v.Value))
            .Build();
}
