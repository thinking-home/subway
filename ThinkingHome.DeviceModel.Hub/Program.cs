using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Hub;
using ThinkingHome.DeviceModel.Remoting.ProxyClient;

// конфиги читаются из каталога сборки (как копирует CopyToOutputDirectory), а не из cwd
var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
});

// конфигурация по общему правилу: appsettings.json (несекретные дефолты) →
// appsettings.{THINKINGHOME_ENVIRONMENT}.json (файл окружения; Development — локальный, вне git) →
// user-secrets → env с префиксом THINKINGHOME_ → командная строка
var environmentName = Environment.GetEnvironmentVariable("THINKINGHOME_ENVIRONMENT");

if (!string.IsNullOrWhiteSpace(environmentName))
{
    // окружение задано явно → его файл обязан существовать (защита от опечатки в имени)
    builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: false);
}

builder.Configuration
    .AddUserSecrets(typeof(Program).Assembly, optional: true)
    .AddEnvironmentVariables("THINKINGHOME_")
    .AddCommandLine(args);

// хост устройств — сердце хаба: одна реализация, обе грани (реестр и потребление)
builder.Services.AddSingleton<DeviceHost>();
builder.Services.AddSingleton<IDeviceHost>(sp => sp.GetRequiredService<DeviceHost>());
builder.Services.AddSingleton<IDeviceRegistry>(sp => sp.GetRequiredService<DeviceHost>());

// плагины — источники устройств; регистрируются раньше коннектора, чтобы их
// hosted-части стартовали до открытия соединения с прокси
HubConfigurator.AddPlugins(builder.Services, builder.Configuration.GetSection("Hub:Plugins"));

// встроенный коннектор облачного прокси — включается, если задан адрес
if (!string.IsNullOrEmpty(builder.Configuration["Proxy:Url"]))
{
    builder.Services.AddSingleton<IOtpDelivery, LogOtpDelivery>();
    builder.Services.AddHostedService<ProxyConnectorService>();
}

var app = builder.Build();

// плагины регистрируют устройства до старта коннекторов,
// чтобы discovery с первого запроса видел полный список
HubConfigurator.RegisterDevices(app.Services);

await app.RunAsync();
