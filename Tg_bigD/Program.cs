using System;

namespace Tg_bigD
{
    class Program
    {
        static void Main(string[] args)
        {
            string token = "1548971882:AAEc7eGMKT-u1TNfzPOo8aq1UG7IM0eHnLc"; // ложим токен в перменную token
            BaseBotClientWrapper client = new BaseBotClientWrapper(token); // создаем экземпляр класса BBCW в client
            client.Start();

            Console.WriteLine("Наберите и отправьте любую строку для завершения работы, когда бот вам надоест.");
            Console.ReadLine();
        }
    }
}
