# ThinkingHome.Alice

Адаптер Яндекс Алисы: маппер нейтральной модели устройств (`ThinkingHome.DeviceModel`) ↔ DTO Яндекса.
Первый и пока единственный адаптер экосистемы. Про само ядро и его связь с Matter — см.
[README ядра](../ThinkingHome.DeviceModel/README.md).

Суть адаптера: нейтральная модель — **гранулярности Matter** (строгие эндпоинты и кластеры), модель
Алисы — **гранулярности UX** (плоский список умений + тип для представления). Маппер — мост между
ними. Весь код маппера — **чистые функции** (без I/O); оркестрацию вызовов хоста делает бридж
(`ThinkingHome.Alice.Service`), не маппер. DTO-классы Алисы (`Model/`) — это **выход маппера**, а не
вход устройства.

## Два направления

Маппер (`Mapping/AliceMapper.cs`) разбит на два региона по направлению:

- **Алиса → ядро (входящее):** действие Алисы → нейтральная команда — `ToCommand`.
- **Ядро → Алиса (исходящее):** discovery (`ToDevices`), состояние (`ToDeviceState`), результат
  действия (`ToCapabilityActionResult` / `ToActionResult`), коды ошибок (`ToErrorCode`).

## Плоская модель и составные id

У Алисы нет эндпоинтов, поэтому каждый нейтральный **endpoint → отдельное устройство Алисы** с
составным id `deviceId#endpointId` (тип `AliceDeviceId`). `ToDevices` разбивает дескриптор по
эндпоинтам, `ToDeviceState` фильтрует снимок по нужному эндпоинту, `ToCommand` проставляет
`EndpointId`; контроллер парсит составной id обратно.

## Словарь преобразований (замкнутый)

Маппинг ограничен **замкнутым словарём** видов преобразования — все детерминированные и чистые.
Тот же список продублирован в doc-комментарии `AliceMapper`, чтобы правило было под рукой у кода.

| Вид | Пример | Где в маппере |
|---|---|---|
| **1:1 relabel** | `OnOff` → `on_off` | ветки `ToCapabilityInfo` / `ToCapabilityState` / `ToCommand` |
| **value-transform** | `Brightness` (int) → `range` 0–100; нормализация единиц | `PercentRange`, `Units.PERCENT` |
| **type ↔ instance** | `ColorRgbState`/`ColorTemperatureState` ↔ `color_setting` `{rgb, temperature_k}` | `switch` по типу в `ToCapabilityState` / `ToCommand` |
| **derivation (1:N)** | `Open` → `range:open` + производное `on_off` (`on = положение > 0`) | `ToCapabilityInfos` / `ToCapabilityStates` (через `SelectMany`) |

### Правила

1. **Ядро — гранулярность Matter** (стабильный референс).
2. **Любой маппинг выражается этим словарём.**
3. **Если не выражается — это дефект ядра, чинится там, а не escape-hatch'ем в маппере.**

Замкнутость словаря — не украшение, а процедура принятия решений: столкнувшись со структурным
расхождением, не пишем произвольный код, а либо выражаем его объявленным видом, либо (если не
выходит) правим гранулярность ядра.

## Механика 1:N (derivation)

Базовый перевод — один-к-одному (`Select`). Чтобы одно умение/значение ядра дало **несколько**
у Алисы, discovery и состояние идут через `SelectMany`:

```
endpoint.Capabilities.SelectMany(ToCapabilityInfos)   // discovery
snapshot.Values.SelectMany(ToCapabilityStates)        // состояние
```

`ToCapabilityInfos`/`ToCapabilityStates` всегда отдают **основной** маппинг плюс, для `OpenCapability`/
`OpenState`, **производное** `on_off`. Для остальных умений — по-прежнему один элемент. Спец-логика
ограничена явным `if (… is OpenCapability)` с пометкой `derivation`.

## Свойства (сенсоры)

Способности (актуаторы) и свойства (read-only сенсоры) у Алисы — разные ветки устройства:
`capabilities[]` и `properties[]` (виды `float` и `event`). Маппер ведёт их параллельно:

- **discovery:** `endpoint.Properties.Select(ToPropertyInfo)` → `Device.Properties`;
- **состояние:** значения одного снимка разъезжаются по веткам по типу значения (`IsPropertyValue`);
  команд у свойств нет — входящее направление их не касается.

Преобразования — те же виды словаря: `1:1 relabel` (`occupancy → motion`, `contact → open`),
`value-transform` (`bool` → значение события: `detected/not_detected`, `opened/closed`; семантика
ядра — Matter Boolean State: true = контакт замкнут = закрыто). Числовые свойства — `float`
с единицей Алисы (`temperature` °C, `humidity` %).

Нейтральные instance'ы свойств и способностей не пересекаются в пределах endpoint'а — кэш хоста
ключуется `(endpoint, instance)`. Поэтому уставка кондиционера — `target_temperature`, а сенсорная
температура — `temperature` (Алисе обе уходят под её собственными instance'ами: `range:temperature`
и `float:temperature`).

## Применённые решения (прецеденты словаря)

- **Цвет — унификация в ядре (правило 3).** Сначала цвет в ядре был разрезан на две способности
  (температура + rgb), и Алисина одна `color_setting` требовала склейки `N:1` — а ещё два разных
  инстанса состояния копились в кэше (`(endpoint, instance)` без вытеснения). Это не вид из словаря →
  чинили в ядре: одна `ColorCapability` (модель + опц. диапазон температуры), подтипы состояний/команд
  (`ColorRgbState`/`ColorTemperatureState`) с **общим** `Instance = "color"` → один слот кэша,
  переключение перезаписывает. В маппере остался чистый `type ↔ instance`, склейка исчезла.

- **Тумблеры — тип-концепт на каждый, не generic `Toggle{instance}`.** У Алисы `toggle` — один
  generic-тип умения, дискриминируемый instance'ом (oscillation, mute, backlight, …). В ядро это
  обобщение не переносится: в Matter нет кластера «toggle» — осцилляция это атрибут Fan Control
  (`RockSetting`), mute и backlight — совсем другие кластеры, то есть каждый тумблер — отдельный
  концепт (правило 1). Поэтому в ядре — дедицированный тип на концепт (`OscillationCapability`,
  bool как OnOff), как уже сделано для `range` (Brightness/Open) и `mode` (FanSpeed/ThermostatMode);
  generic-структура «все тумблеры — bool» живёт только на стороне DTO Алисы. Маппинг — чистый
  `1:1 relabel` → `toggle:oscillation`.

- **Штора — derivation (не тащим Алисизм в ядро).** Алиса даёт «открыть/закрыть» как **отдельное
  умение `on_off` со своим состоянием**. В Matter это команды одного кластера Window Covering
  (`UpOrOpen`/`DownOrClose`), отдельного on/off-**состояния** нет. Поэтому ядро держит **одно** свойство
  `Open` (положение), а Алисино `on_off` (умение + состояние) — `derivation` в маппере
  (`on = положение > 0`). Команду открыть/закрыть штора исполняет как `OnOffCommand` → крайние положения
  (реальная команда, аналог Matter). В ядре нет отдельного on/off-состояния → нет и связки в кэше.

## Как добавить маппинг новой способности

1. Определи **вид** из словаря (обычно `1:1` или `value-transform`; `type ↔ instance` для семейств
   вроде цвета; `derivation` — только для настоящих Алисизмов).
2. Добавь ветку(и) в `ToCapabilityInfo` (discovery), `ToCapabilityState` (состояние), `ToCommand`
   (действие), при необходимости `ToCapabilityActionResult` (результат).
3. Для `derivation (1:N)` расширь `ToCapabilityInfos`/`ToCapabilityStates`, а не отдельные ветки.
4. Если способность **не выражается** ни одним видом — это сигнал править ядро (см. прецедент с
   цветом), а не добавлять произвольную логику в маппер.
5. Покрой тестом в `ThinkingHome.DeviceModel.Tests/AliceMapperTests` и добавь образцы новых типов в
   `AliceMapperCompletenessTests` — тесты полноты сами перечислят забытые ветки и типы по именам.

## Тесты

- `AliceMapperTests` — все виды преобразования (relabel, value-transform, type↔instance, derivation),
  составные id, коды ошибок.
- `AliceMapperCompletenessTests` — механическая полнота маппера (страховка: на каждый тип ядра нет
  реального устройства, «забытая ветка» должна ловиться тестом, а не в проде). Рефлексией по закрытым
  иерархиям: каждый конкретный тип `Capability`/`Property`/`StateValue` имеет образец и маппится без
  исключений; каждое зарегистрированное действие Алисы даёт команду и результат; каждая нейтральная
  команда достижима из действий Алисы; discovery и состояние объявляют одинаковые наборы умений и
  свойств на instance (включая derivation); канонические instance способностей и свойств не
  пересекаются, а слот кэша `(endpoint, instance)` делится типами состояний только по явному допуску
  (цвет). Новый тип без образца или ветки — красный тест с именем типа.
- `DeviceHostTests.Color_switch_overwrites_previous_representation` — регресс на общий слот кэша у цвета.
