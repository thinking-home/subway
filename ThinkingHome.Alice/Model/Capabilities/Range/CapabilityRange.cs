namespace ThinkingHome.Alice.Model.Capabilities.Range;

/// <summary>Описание способности range в discovery.</summary>
public class CapabilityInfoRange : CapabilityInfo<CapabilityRangeParams>
{
}

/// <summary>Состояние range в query/callback.</summary>
public class CapabilityStateRange : CapabilityState<CapabilityStateRangeData>
{
}

/// <summary>Параметры range в action-запросе.</summary>
public class CapabilityActionParamsRange : CapabilityActionParams<CapabilityStateRangeData>
{
}

/// <summary>Результат операции над range в ответе на action.</summary>
public class CapabilityActionResultRange : CapabilityActionResult<CapabilityStateRangeInstance>
{
}
