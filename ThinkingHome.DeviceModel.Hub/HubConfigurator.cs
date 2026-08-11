namespace ThinkingHome.DeviceModel.Hub;

/// <summary>
/// Подключение плагинов из конфигурации. Секция Hub:Plugins — список имён типов; каждый тип
/// реализует <see cref="IDevicePlugin"/>, создаётся через DI (в конструкторе — только
/// DI-зависимости, обычно IConfiguration) и сам регистрирует свои устройства. Плагин,
/// реализующий ещё и IHostedService, поднимается как фоновый сервис.
/// </summary>
internal static class HubConfigurator
{
    public static void AddPlugins(IServiceCollection services, IConfigurationSection section)
    {
        foreach (var entry in section.GetChildren())
        {
            var typeName = entry.Value;
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new InvalidOperationException($"Hub:Plugins[{entry.Key}]: пустое имя типа.");
            }

            var type = ResolveType(typeName, $"Hub:Plugins[{entry.Key}]");

            if (!typeof(IDevicePlugin).IsAssignableFrom(type))
            {
                throw new InvalidOperationException(
                    $"Hub:Plugins[{entry.Key}]: тип {type.FullName} не реализует IDevicePlugin.");
            }

            services.AddSingleton(type);
            services.AddSingleton(typeof(IDevicePlugin), sp => sp.GetRequiredService(type));

            if (typeof(IHostedService).IsAssignableFrom(type))
            {
                services.AddSingleton(typeof(IHostedService), sp => (IHostedService)sp.GetRequiredService(type));
            }
        }
    }

    public static void RegisterDevices(IServiceProvider provider)
    {
        var registry = provider.GetRequiredService<IDeviceRegistry>();
        var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger("ThinkingHome.DeviceModel.Hub");

        foreach (var plugin in provider.GetServices<IDevicePlugin>())
        {
            try
            {
                plugin.RegisterDevices(registry);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Плагин {plugin.GetType().FullName}: не удалось зарегистрировать устройства. {ex.Message}", ex);
            }
        }

        logger.LogInformation("Устройств зарегистрировано: {Count}", provider.GetRequiredService<DeviceHost>().Count);
    }

    private static Type ResolveType(string name, string context)
        => Type.GetType(name, throwOnError: false)
           ?? throw new InvalidOperationException(
               $"{context}: тип '{name}' не найден. Укажите полное имя с именем сборки, " +
               "например 'ThinkingHome.DeviceModel.Drivers.Stubs.StubsPlugin, ThinkingHome.DeviceModel.Drivers.Stubs'.");
}
