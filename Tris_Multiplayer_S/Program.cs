using System;
using System.Threading.Tasks;

namespace Tris_Multiplayer_S
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ServerAccount server = new ServerAccount();
            Database.ConnectionStart();
        }
    }
}
