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

var obj2 = new CapabilityInfoColorSetting
{
    Parameters = new()
    {
        ColorModel = ColorModel.RGB,
        TemperatureK = new() { Min = 1500, Max = 6500 },
        ColorScene = new()
        {
            Scenes = new ColorScene[]
            {
                new() { Id = ColorSceneId.Alarm },
                new() { Id = ColorSceneId.Alice },
                new() { Id = ColorSceneId.Dinner },
                new() { Id = ColorSceneId.Movie },
                new() { Id = ColorSceneId.Night },
                new() { Id = ColorSceneId.Party },
                new() { Id = ColorSceneId.Rest },
            }
        },
    }
};

var capabilities = new CapabilityInfoBase[] { obj1, obj2 };

string jsonString = JsonSerializer.Serialize(capabilities);

Console.WriteLine(jsonString);

var testResult = JsonSerializer.Deserialize<CapabilityInfoBase[]>(testJson1);

if (testResult == null)
{
    throw new Exception();
}

Console.WriteLine(testResult[0] is CapabilityInfoColorSetting);
Console.WriteLine(testResult[0].GetType());
