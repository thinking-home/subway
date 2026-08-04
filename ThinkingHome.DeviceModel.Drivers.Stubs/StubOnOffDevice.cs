using ThinkingHome.DeviceModel;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.State;

namespace ThinkingHome.DeviceModel.Drivers.Stubs;

/// <summary>
/// Временная заглушка On/Off-устройства (лампа/розетка/выключатель) с состоянием в памяти. Способность
/// одна — OnOff, различается только <see cref="DeviceType"/>. Реальные драйверы придут позже; нужна,
/// чтобы поднять домашний хост и проверить сквозной путь до Алисы.
/// </summary>
public sealed class StubOnOffDevice(string id, StubOnOffDeviceConfig config) : IDevice
{
    private bool isOn;

    /// <inheritdoc />
    public string Id => id;

    /// <inheritdoc />
    public event Action<StateChange>? Changed;

    /// <inheritdoc />
    public DeviceDescriptor Describe() => new()
    {
        Id = id,
        Title = config.Title,
        Room = config.Room,
        Manufacturer = new DeviceManufacturer { Name = "ThinkingHome", Model = "stub" },
        Endpoints = [new Endpoint
        {
            Id = 0,
            Type = config.Type,
            Capabilities = [new OnOffCapability { Instance = OnOffCapability.InstanceName }],
        }],
    };

    /// <inheritdoc />
    public Task<DeviceSnapshot> QueryAsync(CancellationToken ct = default)
        => Task.FromResult(new DeviceSnapshot
        {
            DeviceId = id,
            Values = [new OnOffState { Instance = OnOffCapability.InstanceName, Value = isOn }],
        });

    /// <inheritdoc />
    public Task<CommandOutcome> ExecuteAsync(DeviceCommand command, CancellationToken ct = default)
    {
        if (command is OnOffCommand cmd)
        {
            isOn = cmd.Value;
            Console.WriteLine($"[{id}] → {(isOn ? "ВКЛ" : "выкл")}");
            Changed?.Invoke(new StateChange
            {
                DeviceId = id,
                Value = new OnOffState { Instance = OnOffCapability.InstanceName, Value = isOn },
            });
            return Task.FromResult(CommandOutcome.Done);
        }

        return Task.FromResult(CommandOutcome.Unsupported);
    }
}
