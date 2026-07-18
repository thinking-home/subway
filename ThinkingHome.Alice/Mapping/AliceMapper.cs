#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using ThinkingHome.Alice.Model.ActionResult;
using ThinkingHome.Alice.Model.Capabilities;
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
/// Пока покрыты OnOff и яркость (range).
/// </summary>
public static class AliceMapper
{
    // ── discovery: нейтральный дескриптор → по одному Device Алисы на каждый endpoint ──
    public static IEnumerable<AliceDevice> ToDevices(DeviceDescriptor descriptor) =>
        descriptor.Endpoints.Select(endpoint => new AliceDevice
        {
            Id = new AliceDeviceId(descriptor.Id, endpoint.Id).ToAlice(),
            Name = descriptor.Title,
            Room = descriptor.Room,
            Type = ToAliceDeviceType(endpoint.Type),
            Capabilities = endpoint.Capabilities.Select(ToCapabilityInfo).ToArray(),
            DeviceInfo = ToDeviceInfo(descriptor.Manufacturer),
        });

    // ── query: весь снимок устройства + id → DeviceState (только значения нужного endpoint'а) ──
    public static AliceDeviceState ToDeviceState(AliceDeviceId id, DeviceSnapshot snapshot) => new()
    {
        Id = id.ToAlice(),
        Capabilities = snapshot.Values
            .Where(value => value.EndpointId == id.EndpointId)
            .Select(ToCapabilityState)
            .ToArray(),
    };

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
        _ => throw new NotSupportedException($"Нет нейтрального маппинга для {action.GetType().Name}"),
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
        CapabilityActionParamsRange { State.Instance: CapabilityStateRangeInstance.Brightness } => new CapabilityActionResultRange
        {
            State = new CapabilityStateActionResult<CapabilityStateRangeInstance>
            {
                Instance = CapabilityStateRangeInstance.Brightness,
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

    private static CapabilityInfoBase ToCapabilityInfo(Capability capability) => capability switch
    {
        OnOffCapability c => new CapabilityInfoOnOff
        {
            Retrievable = c.Retrievable,
            Reportable = c.Reportable,
            Parameters = new CapabilityInfoOnOffParams { Split = false },
        },
        BrightnessCapability c => new CapabilityInfoRange
        {
            Retrievable = c.Retrievable,
            Reportable = c.Reportable,
            Parameters = new CapabilityRangeParams
            {
                Instance = CapabilityStateRangeInstance.Brightness,
                Unit = Units.PERCENT,
                RandomAccess = true,
                Range = new CapabilityRangeLimits { Min = 0, Max = 100, Precision = 1 },
            },
        },
        _ => throw new NotSupportedException($"Нет маппинга способности {capability.GetType().Name} в Alice"),
    };

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
        DeviceType.OnOffSocket => AliceDeviceType.Socket,
        DeviceType.OnOffSwitch => AliceDeviceType.Switch,
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
}
