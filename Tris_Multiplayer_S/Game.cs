using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Tris_Multiplayer_S
{
    public class Game
    {
        public TcpClient[] clients;
        private bool?[,] tables;
        public Game(TcpClient client1, TcpClient client2)
        {
            tables = new bool?[3, 3];
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    tables[i, j] = null;
                }
            }
            clients = new TcpClient[] { client1, client2 };
        }

        public bool? this[int x,int y]
        {
            get => tables[x, y];
            set => tables[x, y] = value == null || tables[x, y] == null ? value : tables[x, y];
        }

        public bool? Match(int x, int y, bool player)
        {
            this[x, y] = player;
            //Righe
            for (int i = 0; i < tables.GetLength(0); i++)
            {
                bool aux = true;
                for (int j = 0; j < tables.GetLength(1); j++)
                {
                    if (!(this[i, 0].HasValue && this[i, 0] == this[i, j]))
                    {
                        aux = false;
                    }
                }
                if (aux)
                    return this[i, 0];
            }

            //Colonne
            for (int i = 0; i < tables.GetLength(0); i++)
            {
                bool aux = true;
                for (int j = 0; j < tables.GetLength(1); j++)
                {
                    if (!(this[0, i].HasValue && this[0, i] == this[j, i]))
                    {
                        aux = false;
                    }
                }
                if (aux)
                    return this[0, i];
            }

            //Diagonale \
            {
                bool aux = true;
                for (int j = 0; j < tables.GetLength(1); j++)
                {
                    if (!(this[0, 0].HasValue && this[0, 0] == this[j, j]))
                    {
                        aux = false;
                    }
                }
                if (aux)
                    return this[0, 0];
            }

            //Diagonale /
            {
                bool aux = true;
                for (int j = 0, i = tables.GetLength(1) - 1; j < tables.GetLength(1); j++, i--)
                {
                    if (!(this[0, 2].HasValue && this[0, 2] == this[i, j]))
                    {
                        aux = false;
                    }
                }
                if (aux)
                    return this[0, 2];
            }

            return null;
        }

        public bool isPlayer1(TcpClient client)
        {
            return client == clients[0];
        }
    }
}
