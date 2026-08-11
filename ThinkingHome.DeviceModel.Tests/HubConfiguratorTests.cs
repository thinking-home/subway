using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Hub;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Tests;

public class HubConfiguratorTests
{
    [Fact]
    public void Plugin_from_config_registers_its_devices()
    {
        var provider = Build(PluginEntry(0, typeof(TestPlugin)));

        HubConfigurator.RegisterDevices(provider);

        Assert.Equal(1, provider.GetRequiredService<DeviceHost>().Count);
    }

    [Fact]
    public void Hosted_plugin_becomes_hosted_service_with_same_instance()
    {
        var provider = Build(PluginEntry(0, typeof(TestHostedPlugin)));

        var asPlugin = Assert.Single(provider.GetServices<IDevicePlugin>());
        var asHosted = Assert.Single(provider.GetServices<IHostedService>());
        Assert.Same(asPlugin, asHosted); // один экземпляр во всех ролях

        // обычный плагин фоновым сервисом не становится
        var plain = Build(PluginEntry(0, typeof(TestPlugin)));
        Assert.Empty(plain.GetServices<IHostedService>());
    }

    [Fact]
    public void Unknown_plugin_type_fails_with_position()
    {
        var ex = Assert.Throws<InvalidOperationException>(
            () => Build(("Hub:Plugins:0", "No.Such.Plugin, Nowhere")));
        Assert.Contains("Hub:Plugins[0]", ex.Message);
    }

    [Fact]
    public void Type_without_plugin_interface_fails()
    {
        var ex = Assert.Throws<InvalidOperationException>(
            () => Build(PluginEntry(0, typeof(string))));
        Assert.Contains("не реализует IDevicePlugin", ex.Message);
    }

    [Fact]
    public void Empty_plugin_entry_fails()
    {
        var ex = Assert.Throws<InvalidOperationException>(() => Build(("Hub:Plugins:0", "")));
        Assert.Contains("пустое имя типа", ex.Message);
    }

    [Fact]
    public void Plugin_failure_is_wrapped_with_plugin_name()
    {
        var provider = Build(PluginEntry(0, typeof(ThrowingPlugin)));

        var ex = Assert.Throws<InvalidOperationException>(() => HubConfigurator.RegisterDevices(provider));
        Assert.Contains(nameof(ThrowingPlugin), ex.Message);
        Assert.Contains("сломался при регистрации", ex.Message); // исходная причина сохранена
    }

    [Fact]
    public void Two_plugins_register_into_one_host()
    {
        var provider = Build(PluginEntry(0, typeof(TestPlugin)), PluginEntry(1, typeof(SecondPlugin)));

        HubConfigurator.RegisterDevices(provider);

        Assert.Equal(2, provider.GetRequiredService<DeviceHost>().Count);
    }

    private static (string, string?) PluginEntry(int index, Type type)
        => ($"Hub:Plugins:{index}", type.AssemblyQualifiedName);

    /// <summary>Контейнер как в Program хаба: хост устройств + плагины из конфигурации.</summary>
    private static ServiceProvider Build(params (string Key, string? Value)[] values)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(values.ToDictionary(v => v.Key, v => v.Value))
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IConfiguration>(config);
        services.AddSingleton<DeviceHost>();
        services.AddSingleton<IDeviceHost>(sp => sp.GetRequiredService<DeviceHost>());
        services.AddSingleton<IDeviceRegistry>(sp => sp.GetRequiredService<DeviceHost>());

        HubConfigurator.AddPlugins(services, config.GetSection("Hub:Plugins"));

        return services.BuildServiceProvider();
    }

    public sealed class TestPlugin : IDevicePlugin
    {
        public void RegisterDevices(IDeviceRegistry registry) => registry.Register(new FakeDevice("test-1"));
    }

    public sealed class SecondPlugin : IDevicePlugin
    {
        public void RegisterDevices(IDeviceRegistry registry) => registry.Register(new FakeDevice("test-2"));
    }

    public sealed class TestHostedPlugin : IDevicePlugin, IHostedService
    {
        public void RegisterDevices(IDeviceRegistry registry) { }
        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public sealed class ThrowingPlugin : IDevicePlugin
    {
        public void RegisterDevices(IDeviceRegistry registry)
            => throw new InvalidOperationException("сломался при регистрации");
    }

    private sealed class FakeDevice(string id) : IDevice
    {
        public string Id => id;
        public event Action<StateChange>? Changed { add { } remove { } }

        public DeviceDescriptor Describe() => new()
        {
            Id = id,
            Title = id,
            Endpoints = [new Endpoint { Id = 0, Type = DeviceType.OnOffLight }],
        };

        public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
            => Task.FromResult(new DeviceSnapshot { DeviceId = id, Values = [] });

        public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
            => Task.FromResult(CommandOutcome.Unsupported);
    }
}
