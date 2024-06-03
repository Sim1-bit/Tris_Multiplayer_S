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
    public class ServerAccount : Server
    {
        public ServerAccount() : base(1024)
        {

        }

        protected override void AcceptConnections()
        {
            while (true)
            {
                TcpClient acceptedClient = listener.AcceptTcpClient();
                Console.WriteLine("Connessione accettata.");

                Thread serverThread = new Thread(new ParameterizedThreadStart(HandleCommunication));
                serverThread.Start(acceptedClient);
            }
        }

        private void HandleCommunication(object obj)
        {
            TcpClient client = (TcpClient)obj;

            while (true)
            {
                try
                {
                    // Ricevi dati
                    byte[] buffer = new byte[1024];
                    int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Received an array of strings
                    string[] receivedArray = JsonSerializer.Deserialize<string[]>(receivedMessage);
                    User responseObject = null;
                    if (receivedArray.Length == 2)
                    {
                        responseObject = Database.Access(receivedArray[0], receivedArray[1]);
                    }
                    else if (receivedArray.Length == 3)
                    {
                        responseObject = Database.Registration(receivedArray[0], receivedArray[1]);
                    }
                    string[] responseArray = { responseObject.Username, responseObject.Password, responseObject.Win.ToString(), responseObject.Lose.ToString(), responseObject.Tie.ToString() };
                    Console.WriteLine("Name: {0} Password: {1}\n Win: {2} Lose: {3} Tie: {4}", responseObject.Username, responseObject.Password, responseObject.Win, responseObject.Lose, responseObject.Tie);

                    // Invia dati
                    string messageToSend = JsonSerializer.Serialize(responseArray);
                    byte[] data = Encoding.UTF8.GetBytes(messageToSend);
                    client.GetStream().Write(data, 0, data.Length);
                }
                catch (Exception Err)
                {
                    Console.WriteLine(Err.Message);
                    break;
                }
            }
        }
    }
}
