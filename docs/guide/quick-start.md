# Быстрый старт

За десять минут соберём хаб с одним устройством и поуправляем им из кода. Ни облака, ни Алисы, ни настоящего прибора не
понадобится: вместо реле устройство будет писать в консоль.

## Что понадобится

.NET 10 SDK. Проверить, что он установлен:

```bash
dotnet --version
```

## Проект

```bash
dotnet new console -o MyHome
cd MyHome
dotnet add package ThinkingHome.DeviceModel
dotnet add package ThinkingHome.DeviceModel.FluentApi
```

Первый пакет — ядро: модель устройств и хаб. Второй — API для управления устройствами из кода.

## Устройство

Драйвер реализует интерфейс `IDevice`: описывает устройство, отдаёт его состояние, исполняет команды и сообщает об
изменениях. Создайте файл `Lamp.cs`:

```csharp
using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

public sealed class Lamp(string id, string title) : IDevice
{
    private bool isOn;

    public string Id => id;

    // через это событие драйвер сообщает хабу, что состояние изменилось
    public event Action<StateChange>? Changed;

    // описание устройства: что это за прибор и чем в нём можно управлять
    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = title,
        Endpoints =
        [
            new Endpoint
            {
                Id = 0,
                Type = DeviceType.OnOffLight,
                Capabilities = [new OnOffCapability { Instance = OnOffCapability.InstanceName }],
            },
        ],
    };

    // текущее состояние устройства
    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values = [new OnOffState { Instance = OnOffCapability.InstanceName, Value = isOn }],
        });

    // исполнение команды: здесь драйвер обратился бы к прибору
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
    {
        if (command is not OnOffCommand cmd)
        {
            return Task.FromResult(CommandOutcome.Unsupported);
        }

        isOn = cmd.Value;
        Console.WriteLine($"[{title}] {(isOn ? "включена" : "выключена")}");

        Changed?.Invoke(new StateChange
        {
            DeviceId = id,
            Value = new OnOffState { Instance = OnOffCapability.InstanceName, Value = isOn },
        });

        return Task.FromResult(CommandOutcome.Done);
    }
}
```

Устройство описано в терминах модели: тип `OnOffLight` — лампа с одним выключателем, способность `OnOff` — то, чем в ней
можно управлять. Команду драйвер разбирает сопоставлением с образцом: `OnOffCommand` он понимает, остальные отклоняет
результатом `Unsupported`.

## Хаб

Замените содержимое `Program.cs`:

```csharp
using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.FluentApi;

// хаб и подключённое к нему устройство
var host = new DeviceHost();
host.Register(new Lamp("lamp-1", "Лампа в коридоре"));

var lamp = host.Device("lamp-1");

// подписка на изменения состояния
using var subscription = lamp.OnChanged(change => Console.WriteLine($"хаб получил отчёт: {change.Value}"));

// команда и чтение состояния
await lamp.OnOff().TurnOnAsync();
Console.WriteLine($"лампа включена: {await lamp.OnOff().GetAsync()}");

await lamp.OnOff().TurnOffAsync();
Console.WriteLine($"лампа включена: {await lamp.OnOff().GetAsync()}");
```

## Запуск

```bash
dotnet run
```

```
[Лампа в коридоре] включена
хаб получил отчёт: OnOffState { EndpointId = 0, Instance = on_off, Value = True }
лампа включена: True
[Лампа в коридоре] выключена
хаб получил отчёт: OnOffState { EndpointId = 0, Instance = on_off, Value = False }
лампа включена: False
```

В выводе видны все три вида обращений к устройству: команда дошла до драйвера и он напечатал строку, отчёт об изменении
вернулся подписчику, а запрос состояния отдал текущее значение.

## Что получилось

Хаб с одним устройством, которым можно управлять из кода. Дальше он расширяется в стороны, не меняясь внутри:

- **больше устройств** — зарегистрировать в хабе ещё несколько драйверов;
- **настоящий прибор** — заменить печать в консоль обращением к нему по его протоколу;
- **голосовое управление** — подключить адаптер экосистемы и прокси, чтобы дом стал виден снаружи.

## Что дальше

- [Нейтральная модель](/guide/model) — из чего складывается описание устройства и почему оно устроено именно так;
- [Карта пакетов](/packages/) — какие библиотеки за что отвечают.
