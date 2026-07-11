using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel;

/// <summary>
/// Результат исполнения команды. Адаптеры переводят его в свой формат (у Алисы — action_result).
/// </summary>
public sealed record CommandOutcome
{
    public required CommandStatus Status { get; init; }
    public CommandErrorCode? ErrorCode { get; init; }
    public string? ErrorMessage { get; init; }

    /// <summary>Команда выполнена успешно.</summary>
    public static readonly CommandOutcome Done = new() { Status = CommandStatus.Done };

    /// <summary>Команда не поддерживается устройством.</summary>
    public static readonly CommandOutcome Unsupported = new()
    {
        Status = CommandStatus.Error,
        ErrorCode = CommandErrorCode.NotSupported,
    };

    public static CommandOutcome Error(CommandErrorCode code, string? message = null) => new()
    {
        Status = CommandStatus.Error,
        ErrorCode = code,
        ErrorMessage = message,
    };
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommandStatus
{
    Done,
    Error,
}

/// <summary>Нейтральные коды ошибок; маппятся в коды экосистем (например, в error_code Алисы).</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommandErrorCode
{
    DeviceUnreachable,
    DeviceBusy,
    InvalidValue,
    NotSupported,
    NotSupportedInCurrentMode,
    Internal,
}
