using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.ActionResult
{
    /// <summary>Результат выполнения команды (action_result): статус и, при ошибке, её код и описание.</summary>
    public class ActionResult
    {
        /// <summary>Статус исполнения команды: DONE или ERROR.</summary>
        [JsonPropertyName("status")]
        public ActionResultStatus Status { get; set; }

        /// <summary>Код ошибки; заполняется при статусе ERROR.</summary>
        [JsonPropertyName("error_code")]
        public ActionResultErrorCode? ErrorCode { get; set; }

        /// <summary>Текстовое описание ошибки.</summary>
        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; }

        /// <summary>Успешный результат (статус DONE).</summary>
        public static ActionResult Ok => new() { Status = ActionResultStatus.DONE };

        /// <summary>Результат-ошибка с кодом INVALID_VALUE и необязательным описанием.</summary>
        public static ActionResult InvalidValue(string message = null)
        {
            return new ActionResult
            {
                Status = ActionResultStatus.ERROR,
                ErrorCode = ActionResultErrorCode.INVALID_VALUE,
                ErrorMessage = message
            };
        }
    }
}