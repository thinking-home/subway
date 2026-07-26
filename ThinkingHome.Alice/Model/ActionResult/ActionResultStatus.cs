using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.ActionResult
{
    /// <summary>Статус исполнения команды в action_result.</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActionResultStatus
    {
        /// <summary>Команда выполнена успешно.</summary>
        [JsonStringEnumMemberName("DONE")] DONE,
        /// <summary>При выполнении команды произошла ошибка.</summary>
        [JsonStringEnumMemberName("ERROR")] ERROR,
    }
}