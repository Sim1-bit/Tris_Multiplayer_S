using System;
using System.Threading.Tasks;

namespace Tris_Multiplayer_S
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Server server = new Server(1024);
            await server.StartAsync();
        }
    }
}
