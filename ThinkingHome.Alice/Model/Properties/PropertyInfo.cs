using System.Text.Json.Serialization;
using ThinkingHome.Alice.Model.Properties.Event;
using ThinkingHome.Alice.Model.Properties.Float;

namespace ThinkingHome.Alice.Model.Properties;

/// <summary>Базовое описание свойства (сенсора) в discovery; конкретный тип выбирается по дискриминатору "type" (devices.properties.*).</summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PropertyInfoFloat), PropertyType.FLOAT)]
[JsonDerivedType(typeof(PropertyInfoEvent), PropertyType.EVENT)]
public class PropertyInfoBase
{
    /// <summary>Доступно ли чтение значения свойства (запросы query).</summary>
    [JsonPropertyName("retrievable")] public bool Retrievable { get; set; }

    /// <summary>Сообщает ли устройство об изменении значения через Notification API.</summary>
    [JsonPropertyName("reportable")] public bool Reportable { get; set; }
}

/// <summary>Описание свойства с параметрами конкретного вида.</summary>
public abstract class PropertyInfo<TParams> : PropertyInfoBase
{
    /// <summary>Параметры свойства.</summary>
    [JsonPropertyName("parameters")] public TParams Parameters { get; set; }
}
