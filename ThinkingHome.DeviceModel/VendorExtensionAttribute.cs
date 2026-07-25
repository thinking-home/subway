namespace ThinkingHome.DeviceModel;

/// <summary>
/// Пометка вендорского расширения словаря: концепт, которого нет в словаре Matter (аналог
/// manufacturer-specific элементов самого Matter — §7.19, MEI). Допустимо только при реальной
/// потребности и отсутствии концепта в актуальном Matter; семантика описывается так же строго,
/// как у Matter-концептов (нормализованная единица, полный набор, doc-комментарий). Сверка словаря
/// со спекой (CI-гейт по data_model XML) помеченные типы пропускает; непомеченные обязаны
/// соответствовать Matter. Если Matter позже вводит стандартный концепт с той же семантикой —
/// пометка снимается; если семантика расходится — новый тип + deprecation (правило идентификаторов).
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
public sealed class VendorExtensionAttribute : Attribute;
