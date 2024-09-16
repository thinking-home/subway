using System.Text.Json.Serialization;

namespace ThinkingHome.Alice.Model.ActionResult
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActionResultErrorCode
    {
        [JsonStringEnumMemberName("DOOR_OPEN")]
        DOOR_OPEN,

        [JsonStringEnumMemberName("LID_OPEN")] LID_OPEN,

        [JsonStringEnumMemberName("REMOTE_CONTROL_DISABLED")]
        REMOTE_CONTROL_DISABLED,

        [JsonStringEnumMemberName("NOT_ENOUGH_WATER")]
        NOT_ENOUGH_WATER,

        [JsonStringEnumMemberName("LOW_CHARGE_LEVEL")]
        LOW_CHARGE_LEVEL,

        [JsonStringEnumMemberName("CONTAINER_FULL")]
        CONTAINER_FULL,

        [JsonStringEnumMemberName("CONTAINER_EMPTY")]
        CONTAINER_EMPTY,

        [JsonStringEnumMemberName("DRIP_TRAY_FULL")]
        DRIP_TRAY_FULL,

        [JsonStringEnumMemberName("DEVICE_STUCK")]
        DEVICE_STUCK,

        [JsonStringEnumMemberName("DEVICE_OFF")]
        DEVICE_OFF,

        [JsonStringEnumMemberName("FIRMWARE_OUT_OF_DATE")]
        FIRMWARE_OUT_OF_DATE,

        [JsonStringEnumMemberName("NOT_ENOUGH_DETERGENT")]
        NOT_ENOUGH_DETERGENT,

        [JsonStringEnumMemberName("HUMAN_INVOLVEMENT_NEEDED")]
        HUMAN_INVOLVEMENT_NEEDED,

        [JsonStringEnumMemberName("DEVICE_UNREACHABLE")]
        DEVICE_UNREACHABLE,

        [JsonStringEnumMemberName("DEVICE_BUSY")]
        DEVICE_BUSY,

        [JsonStringEnumMemberName("INTERNAL_ERROR")]
        INTERNAL_ERROR,

        [JsonStringEnumMemberName("INVALID_ACTION")]
        INVALID_ACTION,

        [JsonStringEnumMemberName("INVALID_VALUE")]
        INVALID_VALUE,

        [JsonStringEnumMemberName("NOT_SUPPORTED_IN_CURRENT_MODE")]
        NOT_SUPPORTED_IN_CURRENT_MODE,

        [JsonStringEnumMemberName("ACCOUNT_LINKING_ERROR")]
        ACCOUNT_LINKING_ERROR,

        [JsonStringEnumMemberName("DEVICE_NOT_FOUND")]
        DEVICE_NOT_FOUND,
    }
}