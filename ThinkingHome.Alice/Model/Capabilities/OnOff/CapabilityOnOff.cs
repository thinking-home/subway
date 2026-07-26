using ThinkingHome.Alice.Model.ActionResult;

namespace ThinkingHome.Alice.Model.Capabilities.OnOff;

/// <summary>Параметры on_off в action-запросе.</summary>
public class CapabilityActionParamsOnOff : CapabilityActionParams<CapabilityStateOnOffData>
{
}

/// <summary>Описание способности on_off в discovery.</summary>
public class CapabilityInfoOnOff : CapabilityInfo<CapabilityInfoOnOffParams>
{
}

/// <summary>Состояние on_off в query/callback.</summary>
public class CapabilityStateOnOff : CapabilityState<CapabilityStateOnOffData>
{
}

/// <summary>Результат операции над on_off в ответе на action.</summary>
public class CapabilityActionResultOnOff : CapabilityActionResult<CapabilityStateOnOffInstance>
{
}