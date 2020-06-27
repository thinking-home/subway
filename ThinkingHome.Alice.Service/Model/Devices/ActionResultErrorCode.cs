namespace ThinkingHome.Alice.Service.Model.Devices
{
    public enum ActionResultErrorCode
    {
        DEVICE_UNREACHABLE,
        DEVICE_BUSY,
        DEVICE_NOT_FOUND,
        INTERNAL_ERROR,
        INVALID_ACTION,
        INVALID_VALUE,
        NOT_SUPPORTED_IN_CURRENT_MODE,
    }
}