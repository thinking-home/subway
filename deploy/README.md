# Деплой прокси в Яндекс Облако

Стек: VM (Ubuntu) + **Caddy** (авто-HTTPS Let's Encrypt на :80/:443) → **Proxy** как `systemd`-сервис
на `127.0.0.1:8080`. Код Proxy менять не нужно; ключ подписи JWT приходит из env.

## 0. Предпосылки

- Домен: поддомен, который направим на публичный IP VM.
- `yc` CLI на **публичном** Яндекс Облаке (VM должна быть доступна из интернета — Алиса ходит снаружи).
  Проверь профиль: `yc config profile list`; переключить: `yc config profile activate default`.
- SSH-ключ (`~/.ssh/id_ed25519.pub`).

## 1. Создать VM

```bash
yc compute instance create \
  --name thinkinghome-proxy \
  --zone ru-central1-a \
  --platform standard-v3 --cores 2 --core-fraction 20 --memory 2G \
  --create-boot-disk image-family=ubuntu-2404-lts,type=network-ssd,size=20G \
  --network-interface subnet-name=default-ru-central1-a,nat-ip-version=ipv4 \
  --ssh-key ~/.ssh/id_ed25519.pub
```

Запиши публичный IP из вывода (`one_to_one_nat.address`). Если нет сети/подсети `default` —
создай (`yc vpc network create`, `yc vpc subnet create`) или укажи свою.

**Firewall:** открой входящие 22, 80, 443 (security group VM).

## 2. DNS

A-запись `proxy.ТВОЙ-ДОМЕН` → публичный IP VM. Дождись распространения (`dig proxy.ТВОЙ-ДОМЕН`).

## 3. Publish локально (self-contained — .NET на сервере не нужен)

```bash
dotnet publish ThinkingHome.DeviceModel.Proxy -c Release -r linux-x64 \
  --self-contained -p:PublishSingleFile=true -o publish/proxy
```

## 4. Установить Caddy на сервере

```bash
ssh ubuntu@<IP>          # пользователь обычно ubuntu; уточни по образу
sudo apt update && sudo apt install -y debian-keyring debian-archive-keyring apt-transport-https curl
curl -1sLf https://dl.cloudsmith.io/public/caddy/stable/gpg.key | sudo gpg --dearmor -o /usr/share/keyrings/caddy-stable-archive-keyring.gpg
curl -1sLf https://dl.cloudsmith.io/public/caddy/stable/debian.deb.txt | sudo tee /etc/apt/sources.list.d/caddy-stable.list
sudo apt update && sudo apt install -y caddy
sudo useradd -r -s /usr/sbin/nologin thinkinghome
sudo mkdir -p /opt/thinkinghome-proxy /etc/thinkinghome
```

## 5. Скопировать Proxy и конфиги

```bash
# локально:
scp -r publish/proxy/* ubuntu@<IP>:/tmp/proxy/
scp deploy/thinkinghome-proxy.service deploy/Caddyfile ubuntu@<IP>:/tmp/

# на сервере:
sudo mv /tmp/proxy/* /opt/thinkinghome-proxy/
sudo chmod +x /opt/thinkinghome-proxy/ThinkingHome.DeviceModel.Proxy
sudo chown -R thinkinghome:thinkinghome /opt/thinkinghome-proxy

# СВОЙ ключ подписи (не коммитить):
echo "THINKINGHOME_Jwt__SigningKey=$(openssl rand -base64 48)" | sudo tee /etc/thinkinghome/proxy.env
sudo chmod 600 /etc/thinkinghome/proxy.env

# systemd:
sudo mv /tmp/thinkinghome-proxy.service /etc/systemd/system/
sudo systemctl daemon-reload && sudo systemctl enable --now thinkinghome-proxy

# Caddy (подставь домен):
sudo sed -i 's/proxy.example.com/proxy.ТВОЙ-ДОМЕН/' /tmp/Caddyfile
sudo mv /tmp/Caddyfile /etc/caddy/Caddyfile
sudo systemctl reload caddy
```

## 6. Проверка

```bash
curl -s -o /dev/null -w '%{http_code}\n' https://proxy.ТВОЙ-ДОМЕН/service/v1.0   # 200 (moo)
curl -s https://proxy.ТВОЙ-ДОМЕН/oauth/authorize?redirect_uri=x                  # форма привязки
sudo journalctl -u thinkinghome-proxy -f                                          # логи прокси
```

Домашний коннектор подключать к `https://proxy.ТВОЙ-ДОМЕН/hub` с токеном из
`dotnet ... issue-host-token --hostId <id>` (минтить тем же ключом, что в `/etc/thinkinghome/proxy.env`).

## 7. Навык в Яндекс.Диалогах

- Endpoint URL: `https://proxy.ТВОЙ-ДОМЕН/service`
- URL авторизации: `https://proxy.ТВОЙ-ДОМЕН/oauth/authorize`
- URL для токена: `https://proxy.ТВОЙ-ДОМЕН/oauth/token`
- `client_id` / `client_secret` — задать (валидацию на нашей стороне добавим отдельно, п.1 плана).
