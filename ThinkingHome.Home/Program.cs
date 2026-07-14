using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Remoting.ProxyClient;
using ThinkingHome.Home;

var proxyUrl = args.Length > 0 ? args[0] : "http://localhost:5000/hub";

// хост устройств + временные заглушки-лампы
var host = new DeviceHost();
host.Register(new StubLamp("lamp-1", "Лампа в коридоре", "Коридор"));
host.Register(new StubLamp("lamp-2", "Лампа на кухне", "Кухня"));
host.Register(new StubLamp("lamp-3", "Торшер в гостиной", "Гостиная"));

// коннектор к прокси (hub)
await using var connector = new Connector(host, proxyUrl);

Console.WriteLine($"Домашний хост: 3 лампы зарегистрированы, подключаюсь к {proxyUrl} …");

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
