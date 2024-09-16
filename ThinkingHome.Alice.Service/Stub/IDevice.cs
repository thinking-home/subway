using ThinkingHome.Alice.Handlers.DevicesAction;
using ThinkingHome.Alice.Model;
using ThinkingHome.Alice.Model.ActionResult;

namespace ThinkingHome.Alice.Service.Stub;

public interface IDevice
{
    string Id { get; }
    DeviceState GetStateResponse();
    DeviceActionResult MakeAction(DeviceActionParams action);
    Device GetDescription();
}