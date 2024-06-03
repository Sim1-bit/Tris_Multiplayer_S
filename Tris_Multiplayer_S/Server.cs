using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Intrinsics.X86;
using System.IO;

namespace Tris_Multiplayer_S
{
    public abstract class Server
    {
        protected TcpListener listener;
        protected readonly int port;
        public Server(int port) 
        {
            this.port = port;

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            Thread acceptThread = new Thread(new ThreadStart(AcceptConnections));
            acceptThread.Start();
        }

        protected abstract void AcceptConnections();
    }
}
