using Microsoft.Extensions.Configuration;
using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Remoting.ProxyClient;
using ThinkingHome.DeviceModel.Drivers.Stubs;
using ThinkingHome.Home;

// конфигурация по общему правилу: appsettings.json (несекретные дефолты) →
// appsettings.{THINKINGHOME_ENVIRONMENT}.json (файл окружения; Development — локальный, вне git) →
// user-secrets → env с префиксом THINKINGHOME_ → командная строка
var environmentName = Environment.GetEnvironmentVariable("THINKINGHOME_ENVIRONMENT");

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true);

if (!string.IsNullOrWhiteSpace(environmentName))
{
    // окружение задано явно → его файл обязан существовать (защита от опечатки в имени)
    configBuilder.AddJsonFile($"appsettings.{environmentName}.json", optional: false);
}

var config = configBuilder
    .AddUserSecrets(typeof(Program).Assembly, optional: true)
    .AddEnvironmentVariables("THINKINGHOME_")
    .AddCommandLine(args)
    .Build();

var proxyUrl = config["Proxy:Url"]
    ?? throw new InvalidOperationException("Proxy:Url не задан (appsettings.json / THINKINGHOME_Proxy__Url).");
var token = config["Proxy:HostToken"];
if (token is null)
{
    Console.WriteLine("Proxy:HostToken не задан — хаб с авторизацией отклонит подключение. " +
                      "Задайте: dotnet user-secrets set \"Proxy:HostToken\" \"<токен>\" --project ThinkingHome.Home");
}

// хост устройств + временные заглушки (лампа/розетка/выключатель — все на способности OnOff)
var host = new DeviceHost();
host.Register(new StubOnOffDevice("lamp-1", new StubOnOffDeviceConfig { Title = "Лампа в коридоре", Type = DeviceType.OnOffLight, Room = "Коридор" }));
host.Register(new StubOnOffDevice("lamp-2", new StubOnOffDeviceConfig { Title = "Лампа на кухне", Type = DeviceType.OnOffLight, Room = "Кухня" }));
host.Register(new StubOnOffDevice("lamp-3", new StubOnOffDeviceConfig { Title = "Торшер в гостиной", Type = DeviceType.OnOffLight, Room = "Гостиная" }));
host.Register(new StubOnOffDevice("socket-1", new StubOnOffDeviceConfig { Title = "Розетка у стола", Type = DeviceType.OnOffSocket, Room = "Кабинет" }));
host.Register(new StubOnOffDevice("switch-1", new StubOnOffDeviceConfig { Title = "Выключатель бойлера", Type = DeviceType.OnOffSwitch, Room = "Ванная" }));
host.Register(new StubDimmableLamp("dimmer-1", new StubDeviceConfig { Title = "Диммер в спальне", Room = "Спальня" }));
host.Register(new StubColorTemperatureLamp("cct-1", new StubDeviceConfig { Title = "Лампа с подтоном", Room = "Гостиная" }));
host.Register(new StubColorLamp("rgb-1", new StubDeviceConfig { Title = "RGB-лента", Room = "Гостиная" }));
host.Register(new StubCurtain("curtain-1", new StubDeviceConfig { Title = "Штора в спальне", Room = "Спальня" }));
host.Register(new StubFan("fan-1", new StubDeviceConfig { Title = "Вентилятор в спальне", Room = "Спальня" }));
host.Register(new StubAirConditioner("ac-1", new StubDeviceConfig { Title = "Кондиционер в гостиной", Room = "Гостиная" }));
host.Register(new StubClimateSensor("climate-1", new StubDeviceConfig { Title = "Датчик климата", Room = "Кабинет" }));
host.Register(new StubMotionSensor("motion-1", new StubDeviceConfig { Title = "Датчик движения", Room = "Коридор" }));
host.Register(new StubContactSensor("door-1", new StubDeviceConfig { Title = "Датчик двери", Room = "Прихожая" }));
host.Register(new StubWaterLeakSensor("leak-1", new StubDeviceConfig { Title = "Датчик протечки", Room = "Ванная" }));
host.Register(new StubLightSensor("lux-1", new StubDeviceConfig { Title = "Датчик освещённости", Room = "Балкон" }));
host.Register(new StubAirQualitySensor("aqs-1", new StubDeviceConfig { Title = "Датчик качества воздуха", Room = "Кабинет" }));
host.Register(new StubWaterius("waterius-1", new StubDeviceConfig { Title = "Счётчики воды", Room = "Ванная" }));

// коннектор к прокси (hub); JWT хоста — из конфигурации (Proxy:HostToken)
await using var connector = new Connector(host, new LogOtpDelivery(), proxyUrl, () => Task.FromResult(token));

Console.WriteLine($"Зарегистрировано устройств: {host.Count}");
Console.WriteLine($"Подключаюсь к {proxyUrl} …");

var connected = false;
while (!connected)
{
    try
    {
        await connector.StartAsync();
        connected = true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Не удалось подключиться ({ex.Message}); повтор через 5 с …");
        await Task.Delay(TimeSpan.FromSeconds(5));
    }
}

Console.WriteLine($"Подключено к прокси: {connector.State}. Работаю, Ctrl+C для выхода.");
await Task.Delay(Timeout.Infinite);
