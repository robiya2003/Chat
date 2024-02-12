using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public static class PosgreSqlWithCsharp
    {
        public const string CONNECTINGSTRING = "Server=127.0.0.1;Port=5432;Database=telegramchat;User Id=postgres;Password=dfrt43i0";

        public static bool Regisratsiya(string fullname, string username, string password)
        {
            if (Checking(username, password) && password.Length == 8)
            { 

            #region Registratsiya qismi
            string query = $"insert into users(fullname,username,password1) values" +
                $"('{fullname}','{username}','{password}')";
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(CONNECTINGSTRING);
            npgsqlConnection.Open();
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection);
            npgsqlCommand.ExecuteNonQuery();
            npgsqlConnection.Close();
            #endregion
            return true;
            }
            return false;
        }
        public static bool Checking(string username,string password)
        {
            #region
            string query = $"select * from users where username='{username}' and password1='{password}'";
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(CONNECTINGSTRING);
            npgsqlConnection.Open();
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection);
            NpgsqlDataReader? reader = npgsqlCommand.ExecuteReader();
            List<object[]> ResultList = new List<object[]>();
            while (reader.Read())
            {
                object[] objects = new object[reader.FieldCount];
                reader.GetValues(objects);
                ResultList.Add(objects);
            }
            npgsqlConnection.Close();
            if(ResultList.Count == 0)
            {
                return true;
            }
            return false;
            #endregion
        }
        public static List<object[]> GetAllUsers(string username, string password)
        {
              #region
                string query = $"select user_id,username from users where username<>'{username}' and password1<>'{password}'";
                NpgsqlConnection npgsqlConnection = new NpgsqlConnection(CONNECTINGSTRING);
                npgsqlConnection.Open();
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection);
                NpgsqlDataReader? reader = npgsqlCommand.ExecuteReader();
                List<object[]> ResultList = new List<object[]>();
                while (reader.Read())
                {
                    object[] objects = new object[reader.FieldCount];
                    reader.GetValues(objects);
                    ResultList.Add(objects);
                }
                npgsqlConnection.Close();
                return ResultList;
                
                
                #endregion
            
            
        }
        public static void InserMessage(string username, string password,int id,string message)
        {
            #region Registratsiya qismi
            string query = $"insert into allchats(sender_id,receiver_id,message1) values\r\n((select user_id from users where username='{username}' and password1='{password}'),{id},'{message}')\r\n";
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(CONNECTINGSTRING);
            npgsqlConnection.Open();
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection);
            npgsqlCommand.ExecuteNonQuery();
            npgsqlConnection.Close();
            #endregion
        }
        public static void Messagess(string username, string password)
        {
            #region
            string query = $"select user_id,username,message1 from allchats\r\ninner join users on user_id=sender_id\r\nwhere receiver_id=(select user_id from users where username='{username}' and password1='{password}')";
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(CONNECTINGSTRING);
            npgsqlConnection.Open();
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection);
            NpgsqlDataReader? reader = npgsqlCommand.ExecuteReader();
            List<object[]> ResultList = new List<object[]>();
            while (reader.Read())
            {
                object[] objects = new object[reader.FieldCount];
                reader.GetValues(objects);
                ResultList.Add(objects);
            }
            npgsqlConnection.Close();
            foreach (object[] column in ResultList)
            {
                foreach (object row in column)
                {
                    Console.Write(row + " -> ");
                }
                Console.WriteLine("");
            }


            #endregion
        }

    }
}
