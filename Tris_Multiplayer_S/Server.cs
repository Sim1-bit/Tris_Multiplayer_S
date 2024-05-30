using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
//using System.Net.Sockets;
//using System.Text;
///using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Microsoft.VisualBasic.ApplicationServices;
using System.Reflection.Emit;
using System.Text.Json;
using System.Runtime.Intrinsics.X86;


namespace Tris_Multiplayer_S
{
    public class Server
    {
        private readonly int port;

        public Server(int port)
        {
            this.port = port;
            Form1.inizialization.Release();
        }

        public async Task StartAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }
        private async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) 
                    break;

                string receivedJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (receivedJson.StartsWith("["))
                {
                    // Received an array of strings
                    string[] receivedArray = JsonSerializer.Deserialize<string[]>(receivedJson);
                    User aux = new User(receivedArray[0], receivedArray[1]);
                    string responseJson = JsonSerializer.Serialize(aux);
                    byte[] responseBytes = Encoding.UTF8.GetBytes(responseJson);
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                }
                else
                {
                    // Received a single string
                    string receivedString = JsonSerializer.Deserialize<string>(receivedJson);
                    string responseString = "Received: " + receivedString;
                    byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                }
            }
            client.Close();
        }
    }
}
