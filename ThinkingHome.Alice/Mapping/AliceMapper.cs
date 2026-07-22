#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using ThinkingHome.Alice.Model.ActionResult;
using ThinkingHome.Alice.Model.Capabilities;
using ThinkingHome.Alice.Model.Capabilities.ColorSetting;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.Alice.Model.Capabilities.Range;
using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;
using AliceActionResult = ThinkingHome.Alice.Model.ActionResult.ActionResult;
using AliceDevice = ThinkingHome.Alice.Model.Device;
using AliceDeviceInfo = ThinkingHome.Alice.Model.DeviceInfo;
using AliceDeviceState = ThinkingHome.Alice.Model.DeviceState;
using AliceDeviceType = ThinkingHome.Alice.Model.DeviceType;

namespace ThinkingHome.Alice.Mapping;

/// <summary>
/// Перевод нейтральной модели устройств (ThinkingHome.DeviceModel) в DTO Алисы и обратно. Модель
/// Алисы плоская, поэтому каждый нейтральный endpoint → отдельное устройство с составным id
/// (<see cref="AliceDeviceId"/>). Здесь живёт вся специфика формата Яндекса; ядро о ней не знает.
/// Пока покрыты OnOff, range (яркость, положение) и цвет (color_setting).
///
/// Маппинг ограничен замкнутым словарём преобразований (все — детерминированные, чистые функции):
///   • 1:1 relabel      — OnOff → on_off
///   • value-transform  — Brightness (int) → range (0–100)
///   • type ↔ instance  — Color{Rgb,Temperature}State ↔ color_setting {rgb, temperature_k}
///   • derivation (1:N) — Open → range:open + производное on_off (открыто = положение &gt; 0)
/// Правила: (1) ядро — гранулярность Matter; (2) любой маппинг выражается этим словарём; (3) если
/// не выражается — это дефект ядра, чинится там, а не escape-hatch'ем в маппере.
/// </summary>
public static class AliceMapper
{
    #region Алиса → общая модель (входящее)

    // ── action: параметры способности Алисы + endpoint → нейтральная команда ──
    public static DeviceCommand ToCommand(CapabilityActionParamsBase action, int endpointId) => action switch
    {
        CapabilityActionParamsOnOff a => new OnOffCommand
        {
            EndpointId = endpointId,
            Instance = "on",
            Value = a.State.Value,
        },
        CapabilityActionParamsRange { State.Instance: CapabilityStateRangeInstance.Brightness } a => new BrightnessCommand
        {
            EndpointId = endpointId,
            Instance = "brightness",
            Value = (int)a.State.Value,
        },
        CapabilityActionParamsRange { State.Instance: CapabilityStateRangeInstance.Open } a => new OpenCommand
        {
            EndpointId = endpointId,
            Instance = "open",
            Value = (int)a.State.Value,
        },
        CapabilityActionParamsColorSetting { State.Instance: CapabilityColorInstance.TemperatureK } a => new ColorTemperatureCommand
        {
            EndpointId = endpointId,
            Instance = ColorCapability.InstanceName,
            Value = a.State.Value,
        },
        CapabilityActionParamsColorSetting { State.Instance: CapabilityColorInstance.Rgb } a => new ColorRgbCommand
        {
            EndpointId = endpointId,
            Instance = ColorCapability.InstanceName,
            Value = a.State.Value,
        },
        _ => throw new NotSupportedException($"Нет нейтрального маппинга для {action.GetType().Name}"),
    };

    #endregion

    #region Общая модель → Алиса (исходящее)

    // ── discovery: нейтральный дескриптор → по одному Device Алисы на каждый endpoint ──
    public static IEnumerable<AliceDevice> ToDevices(DeviceDescriptor descriptor) =>
        descriptor.Endpoints.Select(endpoint => new AliceDevice
        {
            Id = new AliceDeviceId(descriptor.Id, endpoint.Id).ToAlice(),
            Name = descriptor.Title,
            Room = descriptor.Room,
            Type = ToAliceDeviceType(endpoint.Type),
            Capabilities = endpoint.Capabilities.SelectMany(ToCapabilityInfos).ToArray(),
            DeviceInfo = ToDeviceInfo(descriptor.Manufacturer),
        });

    // ── query: весь снимок устройства + id → DeviceState (только значения нужного endpoint'а) ──
    public static AliceDeviceState ToDeviceState(AliceDeviceId id, DeviceSnapshot snapshot) => new()
    {
        Id = id.ToAlice(),
        Capabilities = snapshot.Values
            .Where(value => value.EndpointId == id.EndpointId)
            .SelectMany(ToCapabilityStates)
            .ToArray(),
    };

    // ── action: результат нейтральной команды → результат способности Алисы ──
    public static CapabilityActionResultBase ToCapabilityActionResult(
        CapabilityActionParamsBase action, CommandOutcome outcome) => action switch
    {
        CapabilityActionParamsOnOff => new CapabilityActionResultOnOff
        {
            State = new CapabilityStateActionResult<CapabilityStateOnOffInstance>
            {
                Instance = CapabilityStateOnOffInstance.On,
                ActionResult = ToActionResult(outcome),
            },
        },
        CapabilityActionParamsRange a => new CapabilityActionResultRange
        {
            State = new CapabilityStateActionResult<CapabilityStateRangeInstance>
            {
                Instance = a.State.Instance,
                ActionResult = ToActionResult(outcome),
            },
        },
        CapabilityActionParamsColorSetting a => new CapabilityActionResultColorSetting
        {
            State = new CapabilityStateActionResult<CapabilityColorInstance>
            {
                Instance = a.State.Instance,
                ActionResult = ToActionResult(outcome),
            },
        },
        _ => throw new NotSupportedException($"Нет маппинга результата для {action.GetType().Name}"),
    };

    // ── общий результат операции ──
    public static AliceActionResult ToActionResult(CommandOutcome outcome) => outcome.Status == CommandStatus.Done
        ? AliceActionResult.Ok
        : new AliceActionResult
        {
            Status = ActionResultStatus.ERROR,
            ErrorCode = ToErrorCode(outcome.ErrorCode),
            ErrorMessage = outcome.ErrorMessage,
        };

    // discovery: одно умение ядра → одно или несколько умений Алисы (виды преобразования — в описании класса)
    private static IEnumerable<CapabilityInfoBase> ToCapabilityInfos(Capability capability)
    {
        yield return ToCapabilityInfo(capability); // базовый маппинг 1:1

        // derivation: у openable положение (range:open) дополняется тумблером on_off
        if (capability is OpenCapability)
            yield return OnOffInfo(capability);
    }

    private static CapabilityInfoOnOff OnOffInfo(Capability c) => new()
    {
        Retrievable = c.Retrievable,
        Reportable = c.Reportable,
        Parameters = new CapabilityInfoOnOffParams { Split = false },
    };

    private static CapabilityInfoBase ToCapabilityInfo(Capability capability) => capability switch
    {
        OnOffCapability c => OnOffInfo(c),
        BrightnessCapability c => PercentRange(c, CapabilityStateRangeInstance.Brightness),
        OpenCapability c => PercentRange(c, CapabilityStateRangeInstance.Open),
        ColorCapability c => new CapabilityInfoColorSetting
        {
            Retrievable = c.Retrievable,
            Reportable = c.Reportable,
            Parameters = new CapabilityColorParams
            {
                ColorModel = c.Model is ColorModel.Rgb ? ColorModels.RGB : null,
                TemperatureK = c.Temperature is { } t
                    ? new CapabilityColorTemperatureRange { Min = t.MinKelvin, Max = t.MaxKelvin }
                    : null,
            },
        },
        _ => throw new NotSupportedException($"Нет маппинга способности {capability.GetType().Name} в Alice"),
    };

    // range 0–100 % (яркость, положение шторы …) — единый вид, различается только instance
    private static CapabilityInfoRange PercentRange(Capability c, CapabilityStateRangeInstance instance) => new()
    {
        Retrievable = c.Retrievable,
        Reportable = c.Reportable,
        Parameters = new CapabilityRangeParams
        {
            Instance = instance,
            Unit = Units.PERCENT,
            RandomAccess = true,
            Range = new CapabilityRangeLimits { Min = 0, Max = 100, Precision = 1 },
        },
    };

    // query: одно значение ядра → одно или несколько состояний Алисы
    private static IEnumerable<CapabilityStateBase> ToCapabilityStates(StateValue value)
    {
        yield return ToCapabilityState(value); // базовый маппинг 1:1

        // derivation: on_off у openable выводится из положения (открыто = положение > 0)
        if (value is OpenState open)
            yield return new CapabilityStateOnOff
            {
                State = new CapabilityStateOnOffData { Instance = CapabilityStateOnOffInstance.On, Value = open.Value > 0 },
            };
    }

    private static CapabilityStateBase ToCapabilityState(StateValue value) => value switch
    {
        OnOffState s => new CapabilityStateOnOff
        {
            State = new CapabilityStateOnOffData { Instance = CapabilityStateOnOffInstance.On, Value = s.Value },
        },
        BrightnessState s => new CapabilityStateRange
        {
            State = new CapabilityStateRangeData { Instance = CapabilityStateRangeInstance.Brightness, Value = s.Value },
        },
        OpenState s => new CapabilityStateRange
        {
            State = new CapabilityStateRangeData { Instance = CapabilityStateRangeInstance.Open, Value = s.Value },
        },
        ColorTemperatureState s => new CapabilityStateColorSetting
        {
            State = new CapabilityStateColorData { Instance = CapabilityColorInstance.TemperatureK, Value = s.Value },
        },
        ColorRgbState s => new CapabilityStateColorSetting
        {
            State = new CapabilityStateColorData { Instance = CapabilityColorInstance.Rgb, Value = s.Value },
        },
        _ => throw new NotSupportedException($"Нет маппинга состояния {value.GetType().Name} в Alice"),
    };

    private static AliceDeviceInfo? ToDeviceInfo(DeviceManufacturer? m) => m is null
        ? null
        : new AliceDeviceInfo
        {
            Manufacturer = m.Name,
            Model = m.Model,
            HardwareVersion = m.HardwareVersion,
            SoftwareVersion = m.SoftwareVersion,
        };

    private static AliceDeviceType ToAliceDeviceType(DeviceType type) => type switch
    {
        DeviceType.OnOffLight => AliceDeviceType.Light,
        DeviceType.DimmableLight => AliceDeviceType.Light,
        DeviceType.ColorTemperatureLight => AliceDeviceType.Light,
        DeviceType.ExtendedColorLight => AliceDeviceType.Light,
        DeviceType.OnOffSocket => AliceDeviceType.Socket,
        DeviceType.OnOffSwitch => AliceDeviceType.Switch,
        DeviceType.Curtain => AliceDeviceType.Curtain,
        _ => throw new NotSupportedException($"Нет маппинга типа устройства {type} в Alice"),
    };

    private static ActionResultErrorCode ToErrorCode(CommandErrorCode? code) => code switch
    {
        CommandErrorCode.DeviceUnreachable => ActionResultErrorCode.DEVICE_UNREACHABLE,
        CommandErrorCode.DeviceBusy => ActionResultErrorCode.DEVICE_BUSY,
        CommandErrorCode.InvalidValue => ActionResultErrorCode.INVALID_VALUE,
        CommandErrorCode.NotSupported => ActionResultErrorCode.INVALID_ACTION,
        CommandErrorCode.NotSupportedInCurrentMode => ActionResultErrorCode.NOT_SUPPORTED_IN_CURRENT_MODE,
        CommandErrorCode.Internal => ActionResultErrorCode.INTERNAL_ERROR,
        _ => ActionResultErrorCode.INTERNAL_ERROR,
    };

    #endregion
}
