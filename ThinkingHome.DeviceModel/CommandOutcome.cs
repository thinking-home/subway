using System.Text.Json.Serialization;

namespace ThinkingHome.DeviceModel;

/// <summary>
/// Результат исполнения команды. Адаптеры переводят его в свой формат (у Алисы — action_result).
/// </summary>
public sealed record CommandOutcome
{
    /// <summary>Статус исполнения.</summary>
    public required CommandStatus Status { get; init; }
    /// <summary>Код ошибки (заполнен при Status = Error).</summary>
    public CommandErrorCode? ErrorCode { get; init; }
    /// <summary>Диагностическое сообщение для журнала (не для показа пользователю).</summary>
    public string? ErrorMessage { get; init; }

    /// <summary>Команда выполнена успешно.</summary>
    public static readonly CommandOutcome Done = new() { Status = CommandStatus.Done };

    /// <summary>Команда не поддерживается устройством.</summary>
    public static readonly CommandOutcome Unsupported = new()
    {
        Status = CommandStatus.Error,
        ErrorCode = CommandErrorCode.NotSupported,
    };

    /// <summary>Результат-ошибка с кодом и необязательным сообщением.</summary>
    public static CommandOutcome Error(CommandErrorCode code, string? message = null) => new()
    {
        Status = CommandStatus.Error,
        ErrorCode = code,
        ErrorMessage = message,
    };
}

/// <summary>Статус исполнения команды.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommandStatus
{
    /// <summary>Команда выполнена.</summary>
    Done,
    /// <summary>Команда завершилась ошибкой (код — в ErrorCode).</summary>
    Error,
}

/// <summary>Нейтральные коды ошибок; маппятся в коды экосистем (например, в error_code Алисы).</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommandErrorCode
{
    /// <summary>Устройство или его хост недоступны.</summary>
    DeviceUnreachable,
    /// <summary>Устройство занято и не может исполнить команду сейчас.</summary>
    DeviceBusy,
    /// <summary>Недопустимое значение команды.</summary>
    InvalidValue,
    /// <summary>Команда не поддерживается устройством.</summary>
    NotSupported,
    /// <summary>Команда не поддерживается в текущем режиме устройства.</summary>
    NotSupportedInCurrentMode,
    /// <summary>Внутренняя ошибка исполнения.</summary>
    Internal,
}
