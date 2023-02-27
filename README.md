# subway

Документация:
- [Протокол работы платформы умного дома](https://yandex.ru/dev/dialogs/smart-home/doc/reference/resources.html)

## Как запустить

```bash
# hub
dotnet run --project=ThinkingHome.Subway.Hub

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

- * Авторизация локального сервера через [jwt]((https://learn.microsoft.com/ru-ru/aspnet/core/signalr/authn-and-authz?view=aspnetcore-7.0)) (hub пишет токент в лог при старте).
- * Авторизация клиентов по id устройства

## todo

- авторизация клиентов
- [авторизация локального сервера]
- предоставлять схему локального сервера

- причесать нейминг и файловую структуру
- расставить `sealed` в классах-наследниках 

## notes

- [про клиент на .NET Core](https://docs.microsoft.com/ru-ru/aspnet/core/signalr/dotnet-client?view=aspnetcore-3.1)
- [навыки Алисы для умного дома](https://yandex.ru/dev/dialogs/alice/doc/smart-home/reference/post-action-docpage/)
- [описание протокола OAuth2](https://www.oauth.com/oauth2-servers/authorization/the-authorization-request/)

### Как завести проект

Нужно создать навык с типом "Умный дом" на платформе [Яндекс Диалоги](https://dialogs.yandex.ru/developer) ([документация](https://yandex.ru/dev/dialogs/smart-home/doc/concepts/quick-start.html)).

В настройках нужно указать адреса:
- Endpoint URL: `https://<domain>/service`
- URL авторизации: `https://<domain>/oauth/authorize`
- URL для получения токена: `https://<domain>/oauth/token`

Сервер должен быть доступен снаружи и работать по https. Работает отладка через https://ngrok.com

### hosting

- https://certbot.eff.org/instructions?ws=other&os=ubuntufocal

получить сертификат: `sudo certbot certonly --standalone`
сконвертировать сертификат: `sudo openssl pkcs12 -in /etc/letsencrypt/live/alice.thinking-home.ru/cert.pem -inkey /etc/letsencrypt/live/alice.thinkin.ru/privkey.pem -export -out merged.pfx`

запустить на сервере: `sudo dotnet run --project=ThinkingHome.Subway.Hub --urls=https://+:443`
