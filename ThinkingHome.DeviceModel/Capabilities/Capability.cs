using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel.Capabilities;

/// <summary>
/// Способность устройства (актуатор). Закрытая иерархия: конкретный тип служит дискриминатором
/// вместо строки. Новая способность добавляется осознанно — словарь выровнен на кластеры Matter.
/// При добавлении наследника обязательно зарегистрировать его в [JsonDerivedType] ниже.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(OnOffCapability), "onOff")]
[JsonDerivedType(typeof(BrightnessCapability), "brightness")]
public abstract record Capability
{
    /// <summary>Экземпляр способности (например, "on", "brightness").</summary>
    public required string Instance { get; init; }

    /// <summary>Можно ли опросить текущее состояние.</summary>
    public bool Retrievable { get; init; } = true;

    /// <summary>Присылает ли устройство изменения состояния (push).</summary>
    public bool Reportable { get; init; } = true;
}
