using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Remoting.ProxyClient;
using ThinkingHome.Home;

var proxyUrl = args.Length > 0 ? args[0] : "https://alice.thinking-home.ru/hub";
var token = args.Length > 1 ? args[1] : Environment.GetEnvironmentVariable("HOST_TOKEN");

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

// коннектор к прокси (hub); JWT хоста — из аргумента или переменной окружения HOST_TOKEN
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
