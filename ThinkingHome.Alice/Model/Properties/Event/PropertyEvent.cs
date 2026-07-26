namespace ThinkingHome.Alice.Model.Properties.Event;

/// <summary>Описание событийного свойства (event) в discovery.</summary>
public class PropertyInfoEvent : PropertyInfo<PropertyEventParams>
{
}

/// <summary>Состояние событийного свойства в query/callback.</summary>
public class PropertyStateEvent : PropertyState<PropertyStateEventData>
{
}
