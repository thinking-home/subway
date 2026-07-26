namespace ThinkingHome.Alice.Model.Properties;

/// <summary>Строковые идентификаторы типов свойств Алисы (devices.properties.*) — значения дискриминатора "type".</summary>
public static class PropertyType
{
    /// <summary>Числовое свойство (показание сенсора).</summary>
    public const string FLOAT = "devices.properties.float";
    /// <summary>Событийное свойство (дискретные события).</summary>
    public const string EVENT = "devices.properties.event";
}
