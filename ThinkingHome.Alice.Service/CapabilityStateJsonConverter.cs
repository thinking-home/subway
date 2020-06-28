using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ThinkingHome.Alice.Service.Model.Devices.Capability;
using ThinkingHome.Alice.Service.Model.Devices.Capability.ColorSetting;
using ThinkingHome.Alice.Service.Model.Devices.Capability.OnOff;

namespace ThinkingHome.Alice.Service
{
    public class CapabilityStateJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(CapabilityStateModelBase).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject json = JObject.Load(reader);
            string instance = json["instance"]?.ToString();

            CapabilityStateModelBase obj = null;

            switch (instance)
            {
                case "on":
                    obj = new OnCapabilityState();
                    break;
                case "hsv":
                    obj = new HsvCapabilityState();
                    break;
            }

            serializer.Populate(json.CreateReader(), obj);

            return obj;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var obj = value as CapabilityStateModelBase;
            var jo = new JObject();
            jo.Add("instance", obj.instance);
            jo.Add("value", JToken.FromObject(obj.SerializeValue(), serializer));
            jo.WriteTo(writer);
        }
    }
}
