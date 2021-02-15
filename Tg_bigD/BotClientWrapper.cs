using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Tg_bigD
{
    class BaseBotClientWrapper
    {
        string token;
        //Экзепляр класс TelegramBotClient, нужен для взаимодействия с телегой.
        TelegramBotClient client;
        public BaseBotClientWrapper(string token)
        {
            this.token = token;
            client = new TelegramBotClient(token);
            client.OnMessage += BotOnMessageReceived;
        }
        /// <summary>
        /// Метод для запуска бота
        /// </summary>
        public void Start()
        {
            client.StartReceiving();
        }
        /// <summary>
        /// Действие бота при получении нового сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="messageEventArgs"></param>
        public virtual void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            Message message = messageEventArgs.Message;


            switch (message.Text)
            {
                // Приветствие для нового пользователя
                case "/start":
                    {
                        client.SendTextMessageAsync(message.Chat.Id, "Привет!"); ;
                        break;
                    }
                default://дефолтный ответ
                    Console.WriteLine(message.Text);
                    client.SendTextMessageAsync(message.Chat.Id, message.Text);
                    break;
            }
            
        }
    }
}
