#nullable enable

using System;

namespace ThinkingHome.Alice.Mapping;

/// <summary>
/// Идентификатор устройства для Алисы: составной <c>deviceId#endpointId</c>. Модель Алисы плоская,
/// поэтому каждый нейтральный endpoint становится отдельным устройством, а endpoint кодируется в id
/// (и восстанавливается при query/action).
/// </summary>
public readonly record struct AliceDeviceId(string DeviceId, int EndpointId)
{
    public string ToAlice() => $"{DeviceId}#{EndpointId}";

    public static AliceDeviceId Parse(string value)
    {
        var i = value.LastIndexOf('#');
        if (i > 0 && int.TryParse(value.AsSpan(i + 1), out var endpointId))
        {
            return new AliceDeviceId(value.Substring(0, i), endpointId);
        }

        return new AliceDeviceId(value, 0); // защита: нет корректного суффикса → endpoint 0
    }
}
