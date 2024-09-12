using System.Text.Json;
using ThinkingHome.Alice.Model.Capabilities;

var testJson1 = @"
[
  {
    ""type"": ""devices.capabilities.on_off"",
    ""retrievable"": false,
    ""reportable"": true,
    ""parameters"": {
      ""split"": true
    }
  },
  {
    ""type"": ""devices.capabilities.color_setting"",
    ""retrievable"": true,
    ""reportable"": false,
    ""parameters"": {
      ""color_model"": ""hsv"",
      ""temperature_k"": {
        ""max"": 6500,
        ""min"": 2700
      },
      ""color_scene"": {
        ""scenes"": [
          { ""id"": ""party"" },
          { ""id"": ""alarm"" },
          { ""id"": ""fantasy"" },
          { ""id"": ""reading"" }
        ]
      }
    }
  }
]
";

var obj1 = new CapabilityInfoOnOff
{
    Reportable = false,
    Retrievable = true,
    Parameters = new()
    {
        Split = true,
    }
};


var capabilities = new CapabilityInfoBase[] { obj1 };

string jsonString = JsonSerializer.Serialize(capabilities);

Console.WriteLine(jsonString);

var testResult = JsonSerializer.Deserialize<CapabilityInfoBase[]>(testJson1);

if (testResult == null)
{
    throw new Exception();
}