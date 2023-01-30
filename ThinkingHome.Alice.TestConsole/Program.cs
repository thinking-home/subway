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
    reportable = false,
    retrievable = true,
    parameters = new()
    {
        split = true,
    }
};

var obj2 = new CapabilityInfoColorSetting
{
    parameters = new()
    {
        color_model = ColorModel.rgb,
        temperature_k = new() { min = 1500, max = 6500 },
        color_scene = new()
        {
            scenes = new ColorScene[]
            {
                new() { id = ColorSceneId.alarm },
                new() { id = ColorSceneId.alice },
                new() { id = ColorSceneId.dinner },
                new() { id = ColorSceneId.movie },
                new() { id = ColorSceneId.night },
                new() { id = ColorSceneId.party },
                new() { id = ColorSceneId.rest },
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
