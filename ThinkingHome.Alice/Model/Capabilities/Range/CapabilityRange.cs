namespace ThinkingHome.Alice.Model.Capabilities.Range;

public class CapabilityInfoRange : CapabilityInfo<CapabilityRangeParams>
{
}

public class CapabilityStateRange : CapabilityState<CapabilityStateRangeData>
{
}

public class CapabilityActionParamsRange : CapabilityActionParams<CapabilityStateRangeData>
{
}

public class CapabilityActionResultRange : CapabilityActionResult<CapabilityStateRangeInstance>
{
}
