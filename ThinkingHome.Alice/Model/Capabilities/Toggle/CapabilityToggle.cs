namespace ThinkingHome.Alice.Model.Capabilities.Toggle;

/// <summary>Описание способности toggle в discovery.</summary>
public class CapabilityInfoToggle : CapabilityInfo<CapabilityToggleParams>
{
}

/// <summary>Состояние toggle в query/callback.</summary>
public class CapabilityStateToggle : CapabilityState<CapabilityStateToggleData>
{
}

/// <summary>Параметры toggle в action-запросе.</summary>
public class CapabilityActionParamsToggle : CapabilityActionParams<CapabilityStateToggleData>
{
}

/// <summary>Результат операции над toggle в ответе на action.</summary>
public class CapabilityActionResultToggle : CapabilityActionResult<CapabilityToggleInstance>
{
}
