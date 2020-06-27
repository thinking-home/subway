using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ThinkingHome.Alice.Service.Model.Devices.Capability;

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
            throw new NotImplementedException();
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
