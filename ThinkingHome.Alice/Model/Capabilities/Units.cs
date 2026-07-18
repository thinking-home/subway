namespace ThinkingHome.Alice.Model.Capabilities;

/// <summary>
/// Единицы измерения Алисы (поле "unit" у range/float). Единицу выбирает конкретная способность:
/// яркость всегда в процентах. По мере добавления инстансов (температура, громкость) сюда добавляются
/// новые константы, а нужную подставляет соответствующая ветка маппера.
/// </summary>
public static class Units
{
    public const string PERCENT = "unit.percent";
}
