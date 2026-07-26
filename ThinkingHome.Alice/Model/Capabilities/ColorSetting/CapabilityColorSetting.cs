namespace ThinkingHome.Alice.Model.Capabilities.ColorSetting;

/// <summary>Описание способности color_setting в discovery.</summary>
public class CapabilityInfoColorSetting : CapabilityInfo<CapabilityColorParams>
{
}

/// <summary>Состояние color_setting в query/callback.</summary>
public class CapabilityStateColorSetting : CapabilityState<CapabilityStateColorData>
{
}

/// <summary>Параметры color_setting в action-запросе.</summary>
public class CapabilityActionParamsColorSetting : CapabilityActionParams<CapabilityStateColorData>
{
}

/// <summary>Результат операции над color_setting в ответе на action.</summary>
public class CapabilityActionResultColorSetting : CapabilityActionResult<CapabilityColorInstance>
{
}
