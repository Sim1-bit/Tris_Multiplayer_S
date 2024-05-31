using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Tris_Multiplayer_S
{
    static class Database
    {
        public static MySqlConnection Connection;
        public static void ConnectionStart()
        {
            if (Connection == null)
            {
                Connection = new MySqlConnection("server=localhost;user=root;database=tris_db;port=3306;password=root");
                Connection.Open();
            }
        }

        public static bool IsAccount(string name)
        {
            string querry = "SELECT name FROM account WHERE name = @name;";
            MySqlCommand cmd = new MySqlCommand(querry, Connection);
            cmd.Parameters.AddWithValue("@name", name);
            MySqlDataReader reader = cmd.ExecuteReader();
            bool aux = reader.Read();
            reader.Close();
            return aux;
        }

        public static bool IsPassowordCorrect(string name, string password)
        {
            if (!IsAccount(name))
                return false;

            string querry = "SELECT password FROM account WHERE name = @name AND password = @password;";
            MySqlCommand cmd = new MySqlCommand(querry, Connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@password", password);
            MySqlDataReader reader = cmd.ExecuteReader();
            bool aux = reader.Read();
            reader.Close();
            return aux;
        }

        public static User Registration(string name, string password)
        {
            if (IsAccount(name))
                return new User("", "");

            string querry = "INSERT INTO account (name, password) VALUES (@name, @password);";
            MySqlCommand cmd = new MySqlCommand(querry, Connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.ExecuteNonQuery();

            querry = "SELECT ID FROM account WHERE name = @name AND password = @password;";
            cmd = new MySqlCommand(querry, Connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@password", password);

            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int ID = reader.GetInt32("ID");
            reader.Close();
            

            querry = "INSERT INTO user (ID, win, lose, tie) VALUES (@ID, 0, 0, 0);";
            cmd = new MySqlCommand(querry, Connection);
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.ExecuteNonQuery();

            return Access(name, password);
        }

        public static User Access(string name, string password)
        {
            if (!IsPassowordCorrect(name, password))
                return new User("", "");
            string querry = "SELECT * FROM account JOIN user WHERE name = @name AND password = @password;";
            MySqlCommand cmd = new MySqlCommand(querry, Connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@password", password);
            MySqlDataReader reader = cmd.ExecuteReader();
            User aux = null;
            if (reader.Read())
            {
                aux = new User(reader["name"].ToString(), reader["password"].ToString(), (int)reader["win"], (int)reader["lose"], (int)reader["tie"]);
            }
            reader.Close();
            return aux;
        }
    }
}
