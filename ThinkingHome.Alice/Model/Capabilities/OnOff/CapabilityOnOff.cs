using ThinkingHome.Alice.Model.ActionResult;

namespace ThinkingHome.Alice.Model.Capabilities.OnOff;

public class CapabilityActionParamsOnOff : CapabilityActionParams<CapabilityStateOnOffData>
{
}

public class CapabilityInfoOnOff : CapabilityInfo<CapabilityInfoOnOffParams>
{
}

public class CapabilityStateOnOff : CapabilityState<CapabilityStateOnOffData>
{
}

public class CapabilityActionResultOnOff : CapabilityActionResult<CapabilityStateOnOffInstance>
{
}