using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Tris_Multiplayer_S
{
    public class ServerGame : Server
    {
        private List<TcpClient> clients = new List<TcpClient>();
        private object clientLock = new object();
        public ServerGame() : base(53000)
        {

        }

        protected override void AcceptConnections()
        {
            while (true)
            {
                TcpClient acceptedClient = listener.AcceptTcpClient();
                Console.WriteLine("Connessione accettata.");

                lock (clientLock)
                {
                    clients.Add(acceptedClient);

                    if (clients.Count % 2 == 0)
                    {
                        TcpClient client1 = clients[clients.Count - 2];
                        TcpClient client2 = clients[clients.Count - 1];
                        Thread clientThread = new Thread(() => HandleCommunication(client1, client2));
                        clientThread.Start();
                    }
                }
            }
        }

        private void HandleCommunication(TcpClient client1, TcpClient client2)
        {
            NetworkStream stream1 = client1.GetStream();
            NetworkStream stream2 = client2.GetStream();

            string messageToSend = JsonSerializer.Serialize(true);
            byte[] data = Encoding.UTF8.GetBytes(messageToSend);
            stream1.Write(data, 0, data.Length);
            messageToSend = JsonSerializer.Serialize(false);
            data = Encoding.UTF8.GetBytes(messageToSend);
            stream2.Write(data, 0, data.Length);

            Thread thread1 = new Thread(() => HandleClient(client1, stream1, stream2));
            Thread thread2 = new Thread(() => HandleClient(client2, stream2, stream1));

            thread1.Start();
            thread2.Start();
        }

        private void HandleClient(TcpClient client, NetworkStream clientStream, NetworkStream peerStream)
        {
            try
            {
                while (true)
                {
                    // Ricevi dati dal client
                    byte[] buffer = new byte[1024];
                    int bytesRead = clientStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        break; // Disconnessione del client
                    }

                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    int[] receivedArray = JsonSerializer.Deserialize<int[]>(receivedMessage);
                    Console.WriteLine(receivedArray);
                    // Invia dati al peer
                    string messageToSend = JsonSerializer.Serialize(receivedArray);
                    byte[] data = Encoding.UTF8.GetBytes(messageToSend);
                    peerStream.Write(data, 0, data.Length);
                }
            }
            catch (Exception Err)
            {
                Console.WriteLine(Err.Message);
            }
            finally
            {
                client.Close();
                clientStream.Close();
            }
        }
    }
}
