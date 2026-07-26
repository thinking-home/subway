# subway

Мост между локальным сервером умного дома и Яндекс Алисой: облачный прокси (hub) даёт
локальному серверу внешний адрес и транслирует запросы платформы умного дома через
SignalR-туннель — без белого IP и проброса портов у пользователя.

> Подробная документация в работе (VitePress-сайт). Пока — README проектов:
> - [ThinkingHome.DeviceModel](ThinkingHome.DeviceModel/README.md) — нейтральная модель устройств (ядро), концепции и словарь
> - [deploy](deploy/README.md) — деплой хаба (VM + Caddy + systemd)

## Как завести навык Алисы

Создайте навык с типом «Умный дом» на платформе [Яндекс Диалоги](https://dialogs.yandex.ru/developer)
([документация](https://yandex.ru/dev/dialogs/smart-home/doc/concepts/quick-start.html)).

В настройках навыка укажите адреса (домен — тот, на котором развёрнут hub):

- Endpoint URL: `https://<domain>/service`
- URL авторизации: `https://<domain>/oauth/authorize`
- URL для получения токена: `https://<domain>/oauth/token`

Сервер должен быть доступен снаружи и работать по HTTPS.
