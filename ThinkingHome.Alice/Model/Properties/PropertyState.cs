using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Properties.Event;
using ThinkingHome.Alice.Model.Properties.Float;

namespace ThinkingHome.Alice.Model.Properties;

/// <summary>Базовое состояние свойства (query/callback); конкретный тип выбирается по дискриминатору "type" (devices.properties.*).</summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PropertyStateFloat), PropertyType.FLOAT)]
[JsonDerivedType(typeof(PropertyStateEvent), PropertyType.EVENT)]
public class PropertyStateBase
{
}

/// <summary>Состояние свойства с данными конкретного вида.</summary>
public abstract class PropertyState<TData> : PropertyStateBase
{
    /// <summary>Состояние свойства: инстанс и значение.</summary>
    [JsonPropertyName("state")] public TData State { get; set; }
}
