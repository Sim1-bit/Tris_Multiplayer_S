using System;
using System.Threading.Tasks;

namespace Tris_Multiplayer_S
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServerAccount server = new ServerAccount();
            Database.ConnectionStart();
            ServerGame game = new ServerGame();
        }
    }
}
