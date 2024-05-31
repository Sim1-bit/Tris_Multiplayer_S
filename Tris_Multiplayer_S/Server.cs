using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tris_Multiplayer_S
{
    public class Server
    {
        private readonly int port;

        public Server(int port)
        {
            this.port = port;
        }

        public async Task StartAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            var stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // Client disconnected

                string receivedJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                if (receivedJson.StartsWith("["))
                {
                    // Received an array of strings
                    string[] receivedArray = JsonSerializer.Deserialize<string[]>(receivedJson);
                    User responseObject = null;
                    if (receivedArray.Length == 2)
                    {
                        responseObject = Database.Access(receivedArray[0], receivedArray[1]);
                    }
                    else if(receivedArray.Length == 3)
                    {
                        responseObject = Database.Registration(receivedArray[0], receivedArray[1]);
                    }
                    Console.WriteLine("Name: {0} Password: {1}\n Win: {2} Lose: {3} Tie: {4}", responseObject.Username, responseObject.Password, responseObject.Win, responseObject.Lose, responseObject.Tie);
                    string responseJson = JsonSerializer.Serialize(responseObject);
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

                await Task.Delay(1000); // Simulate processing delay
            }

            client.Close();
        }
    }
}
