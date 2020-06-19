using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace ThinkingHome.Subway.TestConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/hub")
                .Build();

            var x = false;

            connection.On<string>("xxx", (text) =>
            {
                Console.WriteLine($"Receive: {text}");

                x = text == "moo";
            });

            await connection.StartAsync();
            Console.WriteLine("STarted");

            while (!x)
            {
                Thread.Sleep(500);
            }
        }
    }
}
