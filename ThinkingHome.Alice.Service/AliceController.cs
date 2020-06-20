using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using ThinkingHome.Alice.Service.Model;
using ThinkingHome.Alice.Service.Model.Devices;
using ThinkingHome.Alice.Service.Model.Devices.Capability;

namespace ThinkingHome.Alice.Service
{
    /// <summary>
    /// Общий интерфейс умения
    /// </summary>
    /// <typeparam name="T">Тип поля params</typeparam>
    public abstract class Capability<T, S> where S : CapabilityStateModel
    {
        public abstract CapabilityType Type { get; }
        public abstract bool Retrievable { get; }
        public abstract T Params { get; }
        public abstract S GetState();

        public CapabilityInfoModel GetDescription()
        {
            return new CapabilityInfoModel
            {
                type = Type,
                retrievable = Retrievable,
                parameters = Params
            };
        }

        public CapabilityState GetStateResponse()
        {
            return new CapabilityState
            {
                type = Type,
                state = GetState()
            };
        }
    }

    public class OnOffCapabilityParams
    {
        public bool split { get; set; }
    }

    public class OnCapabilityState : CapabilityStateModel
    {
        public OnCapabilityState(bool value)
        {
            this.value = value;
        }

        public override string instance => "on";

        public override object value { get; }
    }

    public abstract class OnOffCapability : Capability<OnOffCapabilityParams, OnCapabilityState>
    {
        public override CapabilityType Type => CapabilityType.OnOff;
        public abstract bool SplitCommands { get; }

        public abstract bool GetValue();

        public override OnOffCapabilityParams Params => new OnOffCapabilityParams {split = SplitCommands};

        public override OnCapabilityState GetState() => new OnCapabilityState(GetValue());
    }

    public class TestBulbDefaultCapability : OnOffCapability
    {
        public bool Value { get; set; }

        public override bool Retrievable => true;

        public override bool SplitCommands => true;

        public override bool GetValue() => Value;
    }

    public class TestBulb
    {
        private readonly string _id;
        private readonly TestBulbDefaultCapability _xxx;

        public TestBulb(string id)
        {
            _id = id;
            _xxx = new TestBulbDefaultCapability();
        }

        public string Id => _id;

        public DeviceState GetStateResponse()
        {
            return new DeviceState
            {
                id = _id,
                capabilities = new[] {_xxx.GetStateResponse()}
            };
        }

        public DeviceModel GetDescription()
        {
            return new DeviceModel
            {
                id = _id,
                name = "Лампочка",
                type = DeviceType.Light,
                capabilities = new[] {_xxx.GetDescription()},
                device_info = new DeviceInfoModel
                {
                    manufacturer = "little cow",
                    model = "bulb1",
                    hw_version = "1",
                    sw_version = "377549"
                }
            };
        }
    }


    public class AliceController : Controller
    {
        private TestBulb _bulb1 = new TestBulb("1");
        private TestBulb _bulb12 = new TestBulb("12");

        [HttpGet("/service/v1.0")]
        public ActionResult Index()
        {
            return Ok("moo");
        }

        [HttpGet("/service/v1.0/user/unlink")]
        public UnlinkResponse Unlink()
        {
            return new UnlinkResponse {request_id = "123"};
        }

        [HttpGet("/service/v1.0/user/devices")]
        public DevicesResponse Devices()
        {
            return new DevicesResponse
            {
                request_id = "123",
                payload = new DevicesPayload
                {
                    user_id = "dima117a",
                    devices = new[]
                    {
                        _bulb1.GetDescription(),
                        _bulb12.GetDescription(),
                    }
                }
            };
        }

        [HttpPost("/service/v1.0/user/devices/query")]
        public DevicesQueryResponse DevicesQuery(DevicesQueryRequest request)
        {
            Console.WriteLine(request);
            var devices = new List<DeviceState>();

            foreach (var device in request.devices)
            {
                if (device.id == _bulb1.Id)
                {
                    devices.Add(_bulb1.GetStateResponse());
                }
                else if (device.id == _bulb12.Id)
                {
                    devices.Add(_bulb12.GetStateResponse());
                }
            }

            return new DevicesQueryResponse
            {
                request_id = "123",
                payload = new DevicesQueryPayload
                {
                    devices = devices.ToArray()
                }
            };
        }
    }
}