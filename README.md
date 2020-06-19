# subway

## Как запустить

```bash
# hub
dotnet run --project=ThinkingHome.Subway.Server

# client
dotnet run --project=ThinkingHome.Subway.TestConsole
```

## Терминология

- `hub` — приложение, к которому есть доступ из внешней сети, проксирующее запросы на локальный сервер через signalr
- `локальный сервер`
- `клиент`

## Схемы работы

1. Клиент отправляет запрос на hub. Hub сразу отвечает клиенту `OK`. После этого hub пытается передать запрос на локальный сервер с подтверждением доставки.
2. Клиент отправляет запрос на hub. Hub передает запрос на локальный сервер и ждет ответ (с таймаутом). Полученный ответ возвращает клиенту.

- * Авторизация локального сервера через [jwt]((https://docs.microsoft.com/ru-ru/aspnet/core/signalr/authn-and-authz?view=aspnetcore-3.1)) (hub пишет токент в лог при старте).
- * Авторизация клиентов по id устройства

## todo

- авторизация клиентов
- [авторизация локального сервера]
- предоставлять схему локального сервера

- [про клиент на .NET Core](https://docs.microsoft.com/ru-ru/aspnet/core/signalr/dotnet-client?view=aspnetcore-3.1)
- [навыки Алисы для умного дома](https://yandex.ru/dev/dialogs/alice/doc/smart-home/reference/post-action-docpage/)
