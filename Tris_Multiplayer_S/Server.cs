using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Tris_Multiplayer_S
{
    public class Server
    {
        public static void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();

            while (true)
            {
                var client = listener.AcceptTcpClient();
                //STask.Run( () => 
            }
        }

        private static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            //string re
        }
    }
}
