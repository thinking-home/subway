namespace ThinkingHome.Alice.Model.Capabilities.Mode;

/// <summary>Описание способности mode в discovery.</summary>
public class CapabilityInfoMode : CapabilityInfo<CapabilityModeParams>
{
}

/// <summary>Состояние mode в query/callback.</summary>
public class CapabilityStateMode : CapabilityState<CapabilityStateModeData>
{
}

/// <summary>Параметры mode в action-запросе.</summary>
public class CapabilityActionParamsMode : CapabilityActionParams<CapabilityStateModeData>
{
}

/// <summary>Результат операции над mode в ответе на action.</summary>
public class CapabilityActionResultMode : CapabilityActionResult<CapabilityModeInstance>
{
}
