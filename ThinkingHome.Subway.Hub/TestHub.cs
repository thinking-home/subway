using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ThinkingHome.Subway.Hub
{
    public class TestHub: Microsoft.AspNetCore.SignalR.Hub
    {
        public const string CLIENT_METHOD_NAME = "xxx";

        public async Task Yyy(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
