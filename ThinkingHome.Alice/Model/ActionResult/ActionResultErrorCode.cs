using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.ActionResult
{
    /// <summary>Коды ошибок выполнения команды (error_code в action_result).</summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActionResultErrorCode
    {
        /// <summary>Открыта дверца устройства.</summary>
        [JsonStringEnumMemberName("DOOR_OPEN")]
        DOOR_OPEN,

        /// <summary>Открыта крышка устройства.</summary>
        [JsonStringEnumMemberName("LID_OPEN")] LID_OPEN,

        /// <summary>Удалённое управление отключено на устройстве.</summary>
        [JsonStringEnumMemberName("REMOTE_CONTROL_DISABLED")]
        REMOTE_CONTROL_DISABLED,

        /// <summary>Недостаточно воды.</summary>
        [JsonStringEnumMemberName("NOT_ENOUGH_WATER")]
        NOT_ENOUGH_WATER,

        /// <summary>Низкий заряд батареи.</summary>
        [JsonStringEnumMemberName("LOW_CHARGE_LEVEL")]
        LOW_CHARGE_LEVEL,

        /// <summary>Контейнер переполнен.</summary>
        [JsonStringEnumMemberName("CONTAINER_FULL")]
        CONTAINER_FULL,

        /// <summary>Контейнер пуст.</summary>
        [JsonStringEnumMemberName("CONTAINER_EMPTY")]
        CONTAINER_EMPTY,

        /// <summary>Поддон для капель переполнен.</summary>
        [JsonStringEnumMemberName("DRIP_TRAY_FULL")]
        DRIP_TRAY_FULL,

        /// <summary>Устройство застряло.</summary>
        [JsonStringEnumMemberName("DEVICE_STUCK")]
        DEVICE_STUCK,

        /// <summary>Устройство выключено.</summary>
        [JsonStringEnumMemberName("DEVICE_OFF")]
        DEVICE_OFF,

        /// <summary>Прошивка устройства устарела.</summary>
        [JsonStringEnumMemberName("FIRMWARE_OUT_OF_DATE")]
        FIRMWARE_OUT_OF_DATE,

        /// <summary>Недостаточно моющего средства.</summary>
        [JsonStringEnumMemberName("NOT_ENOUGH_DETERGENT")]
        NOT_ENOUGH_DETERGENT,

        /// <summary>Требуется вмешательство человека.</summary>
        [JsonStringEnumMemberName("HUMAN_INVOLVEMENT_NEEDED")]
        HUMAN_INVOLVEMENT_NEEDED,

        /// <summary>Устройство недоступно.</summary>
        [JsonStringEnumMemberName("DEVICE_UNREACHABLE")]
        DEVICE_UNREACHABLE,

        /// <summary>Устройство занято.</summary>
        [JsonStringEnumMemberName("DEVICE_BUSY")]
        DEVICE_BUSY,

        /// <summary>Внутренняя ошибка провайдера.</summary>
        [JsonStringEnumMemberName("INTERNAL_ERROR")]
        INTERNAL_ERROR,

        /// <summary>Команда не поддерживается устройством.</summary>
        [JsonStringEnumMemberName("INVALID_ACTION")]
        INVALID_ACTION,

        /// <summary>Недопустимое значение команды.</summary>
        [JsonStringEnumMemberName("INVALID_VALUE")]
        INVALID_VALUE,

        /// <summary>Команда не поддерживается в текущем режиме устройства.</summary>
        [JsonStringEnumMemberName("NOT_SUPPORTED_IN_CURRENT_MODE")]
        NOT_SUPPORTED_IN_CURRENT_MODE,

        /// <summary>Ошибка связки аккаунтов.</summary>
        [JsonStringEnumMemberName("ACCOUNT_LINKING_ERROR")]
        ACCOUNT_LINKING_ERROR,

        /// <summary>Устройство не найдено.</summary>
        [JsonStringEnumMemberName("DEVICE_NOT_FOUND")]
        DEVICE_NOT_FOUND,
    }
}