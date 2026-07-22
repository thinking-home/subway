using ThinkingHome.Alice.Mapping;
using ThinkingHome.Alice.Model.ActionResult;
using ThinkingHome.Alice.Model.Capabilities.ColorSetting;
using ThinkingHome.Alice.Model.Capabilities.Mode;
using ThinkingHome.Alice.Model.Capabilities.OnOff;
using ThinkingHome.Alice.Model.Capabilities.Range;
using ThinkingHome.Alice.Model.Capabilities.Toggle;
using ThinkingHome.Alice.Model.Properties.Event;
using ThinkingHome.Alice.Model.Properties.Float;
using ThinkingHome.DeviceModel.Capabilities;
using ThinkingHome.DeviceModel.Commands;
using ThinkingHome.DeviceModel.Properties;
using ThinkingHome.DeviceModel.State;
using AliceDeviceType = ThinkingHome.Alice.Model.DeviceType;

namespace ThinkingHome.DeviceModel.Tests;

public class AliceMapperTests
{
    [Fact]
    public void ToDevices_single_endpoint_uses_composite_id()
    {
        var descriptor = new DeviceDescriptor
        {
            Id = "lamp-1",
            Title = "Лампа",
            Room = "Кухня",
            Endpoints = [new Endpoint
            {
                Id = 0,
                Type = DeviceType.OnOffLight,
                Capabilities = [new OnOffCapability { Instance = "on" }],
            }],
        };

        var device = Assert.Single(AliceMapper.ToDevices(descriptor));

        Assert.Equal("lamp-1#0", device.Id);
        Assert.Equal("Лампа", device.Name);
        Assert.Equal("Кухня", device.Room);
        Assert.Equal(AliceDeviceType.Light, device.Type);
        Assert.IsType<CapabilityInfoOnOff>(Assert.Single(device.Capabilities));
    }

    [Fact]
    public void ToDevices_splits_endpoints_into_separate_devices()
    {
        var descriptor = new DeviceDescriptor
        {
            Id = "switch",
            Title = "Выключатель",
            Endpoints =
            [
                new Endpoint { Id = 0, Type = DeviceType.OnOffLight, Capabilities = [new OnOffCapability { Instance = "on" }] },
                new Endpoint { Id = 1, Type = DeviceType.OnOffLight, Capabilities = [new OnOffCapability { Instance = "on" }] },
            ],
        };

        var ids = AliceMapper.ToDevices(descriptor).Select(d => d.Id).ToArray();

        Assert.Equal(new[] { "switch#0", "switch#1" }, ids);
    }

    [Fact]
    public void ToDeviceState_filters_values_by_endpoint()
    {
        var snapshot = new DeviceSnapshot
        {
            DeviceId = "switch",
            Values =
            [
                new OnOffState { EndpointId = 0, Instance = "on", Value = false },
                new OnOffState { EndpointId = 1, Instance = "on", Value = true },
            ],
        };

        var state = AliceMapper.ToDeviceState(new AliceDeviceId("switch", 1), snapshot);

        Assert.Equal("switch#1", state.Id);
        var cap = Assert.IsType<CapabilityStateOnOff>(Assert.Single(state.Capabilities));
        Assert.True(cap.State.Value); // значение именно endpoint'а 1
    }

    [Fact]
    public void ToCommand_sets_endpoint()
    {
        var action = new CapabilityActionParamsOnOff
        {
            State = new CapabilityStateOnOffData { Instance = CapabilityStateOnOffInstance.On, Value = true },
        };

        var command = Assert.IsType<OnOffCommand>(AliceMapper.ToCommand(action, endpointId: 2));

        Assert.Equal(2, command.EndpointId);
        Assert.Equal("on", command.Instance);
        Assert.True(command.Value);
    }

    [Fact]
    public void ToActionResult_maps_done_and_error()
    {
        Assert.Equal(ActionResultStatus.DONE, AliceMapper.ToActionResult(CommandOutcome.Done).Status);

        var error = AliceMapper.ToActionResult(CommandOutcome.Error(CommandErrorCode.DeviceUnreachable, "нет связи"));
        Assert.Equal(ActionResultStatus.ERROR, error.Status);
        Assert.Equal(ActionResultErrorCode.DEVICE_UNREACHABLE, error.ErrorCode);
    }

    [Fact]
    public void AliceDeviceId_roundtrips_and_parses_defensively()
    {
        Assert.Equal("lamp-1#0", new AliceDeviceId("lamp-1", 0).ToAlice());

        var id = AliceDeviceId.Parse("lamp-1#2");
        Assert.Equal("lamp-1", id.DeviceId);
        Assert.Equal(2, id.EndpointId);

        Assert.Equal(new AliceDeviceId("legacy", 0), AliceDeviceId.Parse("legacy"));
    }

    [Theory]
    [InlineData(DeviceType.OnOffLight, AliceDeviceType.Light)]
    [InlineData(DeviceType.DimmableLight, AliceDeviceType.Light)]
    [InlineData(DeviceType.ColorTemperatureLight, AliceDeviceType.Light)]
    [InlineData(DeviceType.ExtendedColorLight, AliceDeviceType.Light)]
    [InlineData(DeviceType.OnOffSocket, AliceDeviceType.Socket)]
    [InlineData(DeviceType.OnOffSwitch, AliceDeviceType.Switch)]
    [InlineData(DeviceType.Curtain, AliceDeviceType.Curtain)]
    [InlineData(DeviceType.Fan, AliceDeviceType.Fan)]
    [InlineData(DeviceType.AirConditioner, AliceDeviceType.ThermostatAc)]
    [InlineData(DeviceType.TemperatureSensor, AliceDeviceType.SensorClimate)]
    [InlineData(DeviceType.HumiditySensor, AliceDeviceType.SensorClimate)]
    [InlineData(DeviceType.OccupancySensor, AliceDeviceType.SensorMotion)]
    [InlineData(DeviceType.ContactSensor, AliceDeviceType.SensorOpen)]
    public void ToDevices_maps_device_types(DeviceType type, AliceDeviceType expected)
    {
        var descriptor = new DeviceDescriptor
        {
            Id = "d",
            Title = "T",
            Endpoints = [new Endpoint { Id = 0, Type = type, Capabilities = [new OnOffCapability { Instance = "on" }] }],
        };

        var device = Assert.Single(AliceMapper.ToDevices(descriptor));
        Assert.Equal(expected, device.Type);
    }

    [Fact]
    public void Brightness_maps_to_alice_range()
    {
        // info: способность → range brightness, 0–100 %
        var descriptor = new DeviceDescriptor
        {
            Id = "d",
            Title = "T",
            Endpoints = [new Endpoint
            {
                Id = 0,
                Type = DeviceType.DimmableLight,
                Capabilities = [new BrightnessCapability { Instance = "brightness" }],
            }],
        };
        var info = Assert.IsType<CapabilityInfoRange>(Assert.Single(Assert.Single(AliceMapper.ToDevices(descriptor)).Capabilities));
        Assert.Equal(CapabilityStateRangeInstance.Brightness, info.Parameters.Instance);
        Assert.Equal("unit.percent", info.Parameters.Unit);
        Assert.Equal(0f, info.Parameters.Range.Min);
        Assert.Equal(100f, info.Parameters.Range.Max);

        // action → команда
        var action = new CapabilityActionParamsRange
        {
            State = new CapabilityStateRangeData { Instance = CapabilityStateRangeInstance.Brightness, Value = 42 },
        };
        var command = Assert.IsType<BrightnessCommand>(AliceMapper.ToCommand(action, endpointId: 3));
        Assert.Equal(3, command.EndpointId);
        Assert.Equal("brightness", command.Instance);
        Assert.Equal(42, command.Value);

        // snapshot → state
        var snapshot = new DeviceSnapshot
        {
            DeviceId = "d",
            Values = [new BrightnessState { EndpointId = 0, Instance = "brightness", Value = 42 }],
        };
        var state = AliceMapper.ToDeviceState(new AliceDeviceId("d", 0), snapshot);
        var rangeState = Assert.IsType<CapabilityStateRange>(Assert.Single(state.Capabilities));
        Assert.Equal(42f, rangeState.State.Value);
    }

    [Fact]
    public void ColorTemperature_maps_to_color_setting()
    {
        // info: color_setting с temperature_k, без color_model
        var info = Assert.IsType<CapabilityInfoColorSetting>(Assert.Single(Assert.Single(AliceMapper.ToDevices(
            Descriptor(DeviceType.ColorTemperatureLight,
                new ColorCapability { Instance = ColorCapability.InstanceName, Temperature = new ColorTemperatureRange { MinKelvin = 2700, MaxKelvin = 6500 } }))).Capabilities));
        Assert.Null(info.Parameters.ColorModel);
        Assert.Equal(2700, info.Parameters.TemperatureK.Min);
        Assert.Equal(6500, info.Parameters.TemperatureK.Max);

        // action → команда
        var action = new CapabilityActionParamsColorSetting
        {
            State = new CapabilityStateColorData { Instance = CapabilityColorInstance.TemperatureK, Value = 3000 },
        };
        var command = Assert.IsType<ColorTemperatureCommand>(AliceMapper.ToCommand(action, endpointId: 0));
        Assert.Equal(3000, command.Value);
        Assert.Equal(ColorCapability.InstanceName, command.Instance);

        // snapshot → state
        var state = AliceMapper.ToDeviceState(new AliceDeviceId("d", 0), new DeviceSnapshot
        {
            DeviceId = "d",
            Values = [new ColorTemperatureState { EndpointId = 0, Instance = ColorCapability.InstanceName, Value = 3000 }],
        });
        var colorState = Assert.IsType<CapabilityStateColorSetting>(Assert.Single(state.Capabilities));
        Assert.Equal(CapabilityColorInstance.TemperatureK, colorState.State.Instance);
        Assert.Equal(3000, colorState.State.Value);
    }

    [Fact]
    public void Rgb_maps_to_color_setting()
    {
        var info = Assert.IsType<CapabilityInfoColorSetting>(Assert.Single(Assert.Single(AliceMapper.ToDevices(
            Descriptor(DeviceType.ExtendedColorLight, new ColorCapability { Instance = ColorCapability.InstanceName, Model = ColorModel.Rgb }))).Capabilities));
        Assert.Equal("rgb", info.Parameters.ColorModel);
        Assert.Null(info.Parameters.TemperatureK);

        var action = new CapabilityActionParamsColorSetting
        {
            State = new CapabilityStateColorData { Instance = CapabilityColorInstance.Rgb, Value = 0xFF0000 },
        };
        var command = Assert.IsType<ColorRgbCommand>(AliceMapper.ToCommand(action, endpointId: 0));
        Assert.Equal(0xFF0000, command.Value);
        Assert.Equal(ColorCapability.InstanceName, command.Instance);
    }

    [Fact]
    public void Color_capability_with_rgb_and_temperature_maps_to_one_color_setting()
    {
        var info = Assert.IsType<CapabilityInfoColorSetting>(Assert.Single(Assert.Single(AliceMapper.ToDevices(
            Descriptor(DeviceType.ExtendedColorLight, new ColorCapability
            {
                Instance = ColorCapability.InstanceName,
                Model = ColorModel.Rgb,
                Temperature = new ColorTemperatureRange { MinKelvin = 2000, MaxKelvin = 9000 },
            }))).Capabilities));
        Assert.Equal("rgb", info.Parameters.ColorModel);
        Assert.Equal(2000, info.Parameters.TemperatureK.Min);
        Assert.Equal(9000, info.Parameters.TemperatureK.Max);
    }

    [Fact]
    public void Open_maps_to_range_and_derives_on_off()
    {
        // discovery: одно OpenCapability → range:open + производное on_off (derivation 1:N)
        var caps = Assert.Single(AliceMapper.ToDevices(
            Descriptor(DeviceType.Curtain, new OpenCapability { Instance = "open" }))).Capabilities;
        var info = Assert.IsType<CapabilityInfoRange>(caps.Single(c => c is CapabilityInfoRange));
        Assert.Equal(CapabilityStateRangeInstance.Open, info.Parameters.Instance);
        Assert.Equal("unit.percent", info.Parameters.Unit);
        Assert.Contains(caps, c => c is CapabilityInfoOnOff);

        // action open → OpenCommand
        var action = new CapabilityActionParamsRange
        {
            State = new CapabilityStateRangeData { Instance = CapabilityStateRangeInstance.Open, Value = 70 },
        };
        var command = Assert.IsType<OpenCommand>(AliceMapper.ToCommand(action, endpointId: 0));
        Assert.Equal(70, command.Value);
        Assert.Equal("open", command.Instance);

        // snapshot: одно OpenState → range + производное on_off (открыто = положение > 0)
        var state = AliceMapper.ToDeviceState(new AliceDeviceId("d", 0), new DeviceSnapshot
        {
            DeviceId = "d",
            Values = [new OpenState { EndpointId = 0, Instance = "open", Value = 70 }],
        });
        var rangeState = Assert.IsType<CapabilityStateRange>(state.Capabilities.Single(c => c is CapabilityStateRange));
        Assert.Equal(70f, rangeState.State.Value);
        var onOff = Assert.IsType<CapabilityStateOnOff>(state.Capabilities.Single(c => c is CapabilityStateOnOff));
        Assert.True(onOff.State.Value); // 70 > 0 → открыто
    }

    [Fact]
    public void FanSpeed_maps_to_alice_mode()
    {
        // info: способность → mode:fan_speed со списком режимов
        var info = Assert.IsType<CapabilityInfoMode>(Assert.Single(Assert.Single(AliceMapper.ToDevices(
            Descriptor(DeviceType.Fan, new FanSpeedCapability { Instance = "fan_speed", Speeds = [FanSpeed.Low, FanSpeed.High] }))).Capabilities));
        Assert.Equal(CapabilityModeInstance.FanSpeed, info.Parameters.Instance);
        Assert.Equal(new[] { CapabilityModeValue.Low, CapabilityModeValue.High }, info.Parameters.Modes.Select(m => m.Value).ToArray());

        // action → команда
        var action = new CapabilityActionParamsMode
        {
            State = new CapabilityStateModeData { Instance = CapabilityModeInstance.FanSpeed, Value = CapabilityModeValue.Medium },
        };
        var command = Assert.IsType<FanSpeedCommand>(AliceMapper.ToCommand(action, endpointId: 0));
        Assert.Equal(FanSpeed.Medium, command.Value);
        Assert.Equal("fan_speed", command.Instance);

        // snapshot → state
        var state = AliceMapper.ToDeviceState(new AliceDeviceId("d", 0), new DeviceSnapshot
        {
            DeviceId = "d",
            Values = [new FanSpeedState { EndpointId = 0, Instance = "fan_speed", Value = FanSpeed.High }],
        });
        var modeState = Assert.IsType<CapabilityStateMode>(Assert.Single(state.Capabilities));
        Assert.Equal(CapabilityModeValue.High, modeState.State.Value);
    }

    [Fact]
    public void Oscillation_maps_to_alice_toggle()
    {
        // info: способность → toggle:oscillation
        var info = Assert.IsType<CapabilityInfoToggle>(Assert.Single(Assert.Single(AliceMapper.ToDevices(
            Descriptor(DeviceType.Fan, new OscillationCapability { Instance = "oscillation" }))).Capabilities));
        Assert.Equal(CapabilityToggleInstance.Oscillation, info.Parameters.Instance);

        // action → команда
        var action = new CapabilityActionParamsToggle
        {
            State = new CapabilityStateToggleData { Instance = CapabilityToggleInstance.Oscillation, Value = true },
        };
        var command = Assert.IsType<OscillationCommand>(AliceMapper.ToCommand(action, endpointId: 0));
        Assert.True(command.Value);
        Assert.Equal("oscillation", command.Instance);

        // snapshot → state
        var state = AliceMapper.ToDeviceState(new AliceDeviceId("d", 0), new DeviceSnapshot
        {
            DeviceId = "d",
            Values = [new OscillationState { EndpointId = 0, Instance = "oscillation", Value = true }],
        });
        var toggleState = Assert.IsType<CapabilityStateToggle>(Assert.Single(state.Capabilities));
        Assert.Equal(CapabilityToggleInstance.Oscillation, toggleState.State.Instance);
        Assert.True(toggleState.State.Value);
    }

    [Fact]
    public void ThermostatMode_maps_to_alice_mode()
    {
        // info: способность → mode:thermostat со списком режимов
        var info = Assert.IsType<CapabilityInfoMode>(Assert.Single(Assert.Single(AliceMapper.ToDevices(
            Descriptor(DeviceType.AirConditioner,
                new ThermostatModeCapability { Instance = "thermostat", Modes = [ThermostatMode.Cool, ThermostatMode.Heat, ThermostatMode.FanOnly] }))).Capabilities));
        Assert.Equal(CapabilityModeInstance.Thermostat, info.Parameters.Instance);
        Assert.Equal(new[] { CapabilityModeValue.Cool, CapabilityModeValue.Heat, CapabilityModeValue.FanOnly },
            info.Parameters.Modes.Select(m => m.Value).ToArray());

        // action → команда
        var action = new CapabilityActionParamsMode
        {
            State = new CapabilityStateModeData { Instance = CapabilityModeInstance.Thermostat, Value = CapabilityModeValue.Dry },
        };
        var command = Assert.IsType<ThermostatModeCommand>(AliceMapper.ToCommand(action, endpointId: 0));
        Assert.Equal(ThermostatMode.Dry, command.Value);
        Assert.Equal("thermostat", command.Instance);

        // snapshot → state
        var state = AliceMapper.ToDeviceState(new AliceDeviceId("d", 0), new DeviceSnapshot
        {
            DeviceId = "d",
            Values = [new ThermostatModeState { EndpointId = 0, Instance = "thermostat", Value = ThermostatMode.Heat }],
        });
        var modeState = Assert.IsType<CapabilityStateMode>(Assert.Single(state.Capabilities));
        Assert.Equal(CapabilityModeInstance.Thermostat, modeState.State.Instance);
        Assert.Equal(CapabilityModeValue.Heat, modeState.State.Value);
    }

    [Fact]
    public void TargetTemperature_maps_to_celsius_range()
    {
        // info: первый непроцентный range — unit celsius, границы из способности
        var info = Assert.IsType<CapabilityInfoRange>(Assert.Single(Assert.Single(AliceMapper.ToDevices(
            Descriptor(DeviceType.AirConditioner,
                new TargetTemperatureCapability { Instance = "target_temperature", MinCelsius = 18, MaxCelsius = 33 }))).Capabilities));
        Assert.Equal(CapabilityStateRangeInstance.Temperature, info.Parameters.Instance);
        Assert.Equal("unit.temperature.celsius", info.Parameters.Unit);
        Assert.Equal(18f, info.Parameters.Range.Min);
        Assert.Equal(33f, info.Parameters.Range.Max);
        Assert.Equal(1f, info.Parameters.Range.Precision);

        // action → команда
        var action = new CapabilityActionParamsRange
        {
            State = new CapabilityStateRangeData { Instance = CapabilityStateRangeInstance.Temperature, Value = 24 },
        };
        var command = Assert.IsType<TargetTemperatureCommand>(AliceMapper.ToCommand(action, endpointId: 0));
        Assert.Equal(24, command.Value);
        Assert.Equal("target_temperature", command.Instance);

        // snapshot → state
        var state = AliceMapper.ToDeviceState(new AliceDeviceId("d", 0), new DeviceSnapshot
        {
            DeviceId = "d",
            Values = [new TargetTemperatureState { EndpointId = 0, Instance = "target_temperature", Value = 24 }],
        });
        var rangeState = Assert.IsType<CapabilityStateRange>(Assert.Single(state.Capabilities));
        Assert.Equal(CapabilityStateRangeInstance.Temperature, rangeState.State.Instance);
        Assert.Equal(24f, rangeState.State.Value);
    }

    [Fact]
    public void Climate_properties_map_to_alice_float()
    {
        // discovery: температура + влажность → float:temperature (celsius) + float:humidity (percent)
        var device = Assert.Single(AliceMapper.ToDevices(new DeviceDescriptor
        {
            Id = "d",
            Title = "T",
            Endpoints = [new Endpoint
            {
                Id = 0,
                Type = DeviceType.TemperatureSensor,
                Properties =
                [
                    new TemperatureProperty { Instance = "temperature" },
                    new HumidityProperty { Instance = "humidity" },
                ],
            }],
        }));
        Assert.Equal(AliceDeviceType.SensorClimate, device.Type);
        Assert.Empty(device.Capabilities);
        Assert.Collection(device.Properties,
            p =>
            {
                var f = Assert.IsType<PropertyInfoFloat>(p);
                Assert.Equal(PropertyFloatInstance.Temperature, f.Parameters.Instance);
                Assert.Equal("unit.temperature.celsius", f.Parameters.Unit);
            },
            p =>
            {
                var f = Assert.IsType<PropertyInfoFloat>(p);
                Assert.Equal(PropertyFloatInstance.Humidity, f.Parameters.Instance);
                Assert.Equal("unit.percent", f.Parameters.Unit);
            });

        // snapshot → properties (не capabilities)
        var state = AliceMapper.ToDeviceState(new AliceDeviceId("d", 0), new DeviceSnapshot
        {
            DeviceId = "d",
            Values =
            [
                new TemperatureState { EndpointId = 0, Instance = "temperature", Value = 23.5 },
                new HumidityState { EndpointId = 0, Instance = "humidity", Value = 41 },
            ],
        });
        Assert.Empty(state.Capabilities);
        Assert.Collection(state.Properties,
            p =>
            {
                var f = Assert.IsType<PropertyStateFloat>(p);
                Assert.Equal(PropertyFloatInstance.Temperature, f.State.Instance);
                Assert.Equal(23.5f, f.State.Value);
            },
            p =>
            {
                var f = Assert.IsType<PropertyStateFloat>(p);
                Assert.Equal(PropertyFloatInstance.Humidity, f.State.Instance);
                Assert.Equal(41f, f.State.Value);
            });
    }

    [Fact]
    public void Occupancy_and_contact_map_to_alice_events()
    {
        // discovery: occupancy → event:motion (relabel), contact → event:open (relabel)
        var motionInfo = Assert.IsType<PropertyInfoEvent>(Assert.Single(Assert.Single(AliceMapper.ToDevices(new DeviceDescriptor
        {
            Id = "d",
            Title = "T",
            Endpoints = [new Endpoint { Id = 0, Type = DeviceType.OccupancySensor, Properties = [new OccupancyProperty { Instance = "occupancy" }] }],
        })).Properties));
        Assert.Equal(PropertyEventInstance.Motion, motionInfo.Parameters.Instance);
        Assert.Equal(new[] { PropertyEventValue.Detected, PropertyEventValue.NotDetected },
            motionInfo.Parameters.Events.Select(e => e.Value).ToArray());

        // state: bool → значение события (value-transform)
        var state = AliceMapper.ToDeviceState(new AliceDeviceId("d", 0), new DeviceSnapshot
        {
            DeviceId = "d",
            Values =
            [
                new OccupancyState { EndpointId = 0, Instance = "occupancy", Value = true },
                new ContactState { EndpointId = 0, Instance = "contact", Value = true },
            ],
        });
        Assert.Collection(state.Properties,
            p =>
            {
                var e = Assert.IsType<PropertyStateEvent>(p);
                Assert.Equal(PropertyEventInstance.Motion, e.State.Instance);
                Assert.Equal(PropertyEventValue.Detected, e.State.Value); // true → движение обнаружено
            },
            p =>
            {
                var e = Assert.IsType<PropertyStateEvent>(p);
                Assert.Equal(PropertyEventInstance.Open, e.State.Instance);
                Assert.Equal(PropertyEventValue.Closed, e.State.Value); // семантика Matter: контакт замкнут → закрыто
            });
    }

    private static DeviceDescriptor Descriptor(DeviceType type, params Capability[] capabilities) => new()
    {
        Id = "d",
        Title = "T",
        Endpoints = [new Endpoint { Id = 0, Type = type, Capabilities = capabilities }],
    };
}
