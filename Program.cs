namespace Chat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CHAT();
        }
        public static void SignUp()
        {
            #region
            Console.WriteLine("parol 8 ta harfdan iborat bolishi \nusername unikal bolishi kerak\n");
            Console.WriteLine("Registratsiya fullname kiriting");
            string fullname = Console.ReadLine();
            Console.WriteLine("Registratsiya uchun username kiriting ");
            string username = Console.ReadLine();
            Console.WriteLine("Registratsiya password kiriting");
            string password = Console.ReadLine();
            if (PosgreSqlWithCsharp.Regisratsiya(fullname, username, password))
            {
                Console.WriteLine("Registartsiyadan muvaffaqiyatli otdingiz");
                CHAT();
            }
            else
            {
                Console.WriteLine("Username yoki parol hato \nQaytadan urinib ko'ring ");
                SignUp();
            }
            #endregion
        }
        public static void LogIn()
        {
            
            Console.Clear();
            Console.WriteLine("LOG IN");
            Console.WriteLine("<- orqaga     oldinga->");
            var key1 = Console.ReadKey();

            if (key1.Key == ConsoleKey.LeftArrow)
            {
                CHAT();
            }
            #region
            Console.WriteLine("username kiriting ");
            string username = Console.ReadLine();
            Console.WriteLine("password kiriting");
            string password = Console.ReadLine();
            if (!PosgreSqlWithCsharp.Checking(username, password))
            {
                chatsss(username, password);
            }
            else
            {
                Console.WriteLine("username yoki password hato");
                LogIn();
            }
            #endregion
        }
        public static void CHAT()
        {
            #region
            //Console.Clear();
            Console.WriteLine("<-  SIGN UP      LOG IN  ->");
            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.LeftArrow)
            {
                SignUp();
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                LogIn();
            }
            #endregion
        }
        public static void Message(string username,string password)
        {
            Console.WriteLine("Yozmoqchi bolgan userni idsini kiriting");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Message kiriting ");
            string message = Console.ReadLine();
            PosgreSqlWithCsharp.InserMessage(username, password, id,message);
            Console.WriteLine("message jonatildi");
            chatsss(username, password);

        }
        public static void chatsss(string username, string password)
        {
            Console.Clear();
            Console.WriteLine("<- orqaga     Message->");
            Console.WriteLine("Messges-----------------------------------");
            PosgreSqlWithCsharp.Messagess(username, password);
            Console.WriteLine("ALL USERS----------------------------------");
            List<object[]> ResultList = PosgreSqlWithCsharp.GetAllUsers(username, password);
            foreach (object[] column in ResultList)
            {
                foreach (object row in column)
                {
                    Console.Write(row + "  ");
                }
                Console.WriteLine("");
            }

            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.LeftArrow)
            {
                LogIn();
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                Message(username, password);
            }
        }
    }
}
