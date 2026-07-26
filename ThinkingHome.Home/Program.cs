using Microsoft.Extensions.Configuration;
using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Remoting.ProxyClient;
using ThinkingHome.Home;

// конфигурация по общему правилу: appsettings.json (несекретные дефолты) → user-secrets
// (локальные секреты разработчика) → env с префиксом THINKINGHOME_ → командная строка
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
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
host.Register(new StubOnOffDevice("lamp-1", "Лампа в коридоре", DeviceType.OnOffLight, "Коридор"));
host.Register(new StubOnOffDevice("lamp-2", "Лампа на кухне", DeviceType.OnOffLight, "Кухня"));
host.Register(new StubOnOffDevice("lamp-3", "Торшер в гостиной", DeviceType.OnOffLight, "Гостиная"));
host.Register(new StubOnOffDevice("socket-1", "Розетка у стола", DeviceType.OnOffSocket, "Кабинет"));
host.Register(new StubOnOffDevice("switch-1", "Выключатель бойлера", DeviceType.OnOffSwitch, "Ванная"));
host.Register(new StubDimmableLamp("dimmer-1", "Диммер в спальне", "Спальня"));
host.Register(new StubColorTemperatureLamp("cct-1", "Лампа с подтоном", "Гостиная"));
host.Register(new StubColorLamp("rgb-1", "RGB-лента", "Гостиная"));
host.Register(new StubCurtain("curtain-1", "Штора в спальне", "Спальня"));
host.Register(new StubFan("fan-1", "Вентилятор в спальне", "Спальня"));
host.Register(new StubAirConditioner("ac-1", "Кондиционер в гостиной", "Гостиная"));
host.Register(new StubClimateSensor("climate-1", "Датчик климата", "Кабинет"));
host.Register(new StubMotionSensor("motion-1", "Датчик движения", "Коридор"));
host.Register(new StubContactSensor("door-1", "Датчик двери", "Прихожая"));
host.Register(new StubWaterLeakSensor("leak-1", "Датчик протечки", "Ванная"));
host.Register(new StubLightSensor("lux-1", "Датчик освещённости", "Балкон"));
host.Register(new StubAirQualitySensor("aqs-1", "Датчик качества воздуха", "Кабинет"));
host.Register(new StubWaterius("waterius-1", "Счётчики воды", "Ванная"));

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
