using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public static class PosgreSqlWithCsharp
    {
        public const string CONNECTINGSTRING = "Server=127.0.0.1;Port=5432;Database=telegramchat;User Id=postgres;Password=dfrt43i0";

        public static bool Regisratsiya(string fullname, string username, string password)
        {
                #region Registratsiya qismi
                byte[]? salt = RandomNumberGenerator.GetBytes(64);

            string query = $"insert into users(fullname,username,password1,salt) values" +
                $"('{fullname}','{username}','{HashPasswordwithSalt(password, Convert.ToHexString(salt))}','{Convert.ToHexString(salt)}')";
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(CONNECTINGSTRING);
            npgsqlConnection.Open();
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, npgsqlConnection);
            npgsqlCommand.ExecuteNonQuery();
            npgsqlConnection.Close();
            #endregion
            return true;
        }
        
        public static bool Checking(string username,string password)
        {
            #region
            string query = $"select * from users where username='{username}' and password1='{HashPasswordwithSalt(password,ReturnSalt(username))}'";
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
                string query = $"select user_id,username from users where username<>'{username}' and password1<>'{HashPasswordwithSalt(password, ReturnSalt(username))}'";
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
        //
        public static void InserMessage(string username, string password,int id,string message)
        {
            #region Registratsiya qismi
            string query = $"insert into allchats(sender_id,receiver_id,message1) values\r\n((select user_id from users where username='{username}' and password1='{HashPasswordwithSalt(password, ReturnSalt(username))}'),{id},'{message}')\r\n";
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
            string query = $"select user_id,username,message1 from allchats\r\ninner join users on user_id=sender_id\r\nwhere receiver_id=(select user_id from users where username='{username}' and password1='{HashPasswordwithSalt(password, ReturnSalt(username))}')";
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
            int n = 0;
            foreach (object[] column in ResultList)
            {
                foreach (object row in column)
                {
                    n++;
                    Console.Write(" "+ row+" ");
                    if (n == 2)
                    {
                        Console.Write(" ->");
                    }
                }
                Console.WriteLine("");
            }


            #endregion
        }
        public static string HashPasswordwithSalt(string password, string salt_)
        {
            #region
            int iterations = 350_000;
            int keySize = 64;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            byte[] salt=Encoding.UTF8.GetBytes(salt_);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                hashAlgorithm,
                outputLength: keySize);
            //Console.WriteLine(Convert.ToHexString(salt));
            //Console.WriteLine(Convert.ToHexString(hash));

            return Convert.ToHexString(hash);
            #endregion
        }
        public static string ReturnSalt(string username)
        {
            #region
            string query = $"select salt from users where username='{username}'";
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
            return ResultList[0][0].ToString();
            #endregion
        }
    }
}
