#!/usr/bin/env bash
# Генерация markdown-справочника API из XML-доков публикуемых библиотек (DefaultDocumentation).
# Результат: docs/reference/<Сборка>/*.md — в git НЕ коммитится, генерится локально и в CI.
# Два прохода: первый собирает links-файлы всех сборок, второй перегенерирует страницы
# со ссылками между сборками (например, справка FluentApi ссылается на типы ядра).
set -euo pipefail
cd "$(dirname "$0")/.."

ASSEMBLIES=(
    ThinkingHome.DeviceModel
    ThinkingHome.DeviceModel.FluentApi
    ThinkingHome.DeviceModel.Remoting.ProxyClient
    ThinkingHome.DeviceModel.Remoting.ProxyServer
    ThinkingHome.DeviceModel.Drivers.Stubs
    ThinkingHome.Alice
)

dotnet build ThinkingHome.DeviceModel.sln -c Release

REF=docs/reference
LINKS=$REF/.links
rm -rf "$REF"
mkdir -p "$LINKS"

generate() { # $1 — сборка; $2 — "extern": добавить ссылки на остальные сборки
    local asm="$1" extern_args=()
    if [[ "${2:-}" == "extern" ]]; then
        for other in "${ASSEMBLIES[@]}"; do
            [[ "$other" == "$asm" ]] || extern_args+=("$LINKS/$other.txt")
        done
        extern_args=(-e "${extern_args[@]}")
    fi
    dotnet tool run defaultdocumentation -- \
        --LogLevel Warning \
        -a "$asm/bin/Release/net10.0/$asm.dll" \
        -o "$REF/$asm" \
        -g Assembly,Namespaces,Types \
        -l "$LINKS/$asm.txt" \
        -b "../$asm/" \
        ${extern_args[@]+"${extern_args[@]}"} # безопасное разворачивание пустого массива под set -u (bash 3.2)
}

for asm in "${ASSEMBLIES[@]}"; do generate "$asm"; done
for asm in "${ASSEMBLIES[@]}"; do generate "$asm" extern; done

rm -rf "$LINKS"
echo "Готово: $REF ($(find "$REF" -name '*.md' | wc -l | tr -d ' ') страниц)"
