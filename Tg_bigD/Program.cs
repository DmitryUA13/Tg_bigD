using System;

namespace Tg_bigD
{
    class Program
    {
        static void Main(string[] args)
        {
            string token = "1505443766:AAGq2QxxbVWKCsJ3CeXPGEmDq4C4RvVuSRg";
            BaseBotClientWrapper client = new BaseBotClientWrapper(token);
            client.Start();

            Console.WriteLine("Наберите и отправьте любую строку для завершения работы, когда бот вам надоест.");
            Console.ReadLine();
        }
    }
}
