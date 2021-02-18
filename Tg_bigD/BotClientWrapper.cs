using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tg_bigD
{
    class BaseBotClientWrapper
    {
        string token;
        //Экзепляр класс TelegramBotClient, нужен для взаимодействия с телегой.
        TelegramBotClient client;
        public BaseBotClientWrapper(string token) /// метод
        {
            this.token = token;
            client = new TelegramBotClient(token);
            client.OnMessage += BotOnMessageReceived;
        }
        /// <summary>
        /// Метод для запуска бота
        /// </summary>
        /// 
        
        public void Start() /// метод
        {
            client.StartReceiving();
        }
        /// <summary>
        /// Действие бота при получении нового сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="messageEventArgs"></param>
        
        public virtual void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs) /// метод
        {
            Message message = messageEventArgs.Message;
            
            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
            {
                client.SendPhotoAsync(message.Chat.Id, message.Photo[0].FileId);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                client.SendTextMessageAsync(message.Chat.Id, message.Text);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                client.SendDocumentAsync(message.Chat.Id, message.Document.FileId);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Audio)
            {
                client.SendAudioAsync(message.Chat.Id, message.Audio.FileId);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Sticker)
            {
                client.SendStickerAsync(message.Chat.Id, message.Sticker.FileId);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Contact)
            {
                client.SendStickerAsync(message.Chat.Id, message.Contact.FirstName);
            }
                        /*switch (message.Type)
            {
                // Приветствие для нового пользователя
                case "/start":
                        client.SendTextMessageAsync(message.Chat.Id, "Привет!");
                        client.SendTextMessageAsync(message.Chat.Id, message.Text);
                        break;
                case message.Type == Telegram.Bot.Types.Enums.MessageType.Photo:
                    {
                        client.SendPhotoAsync(message.Chat.Id, send_default);
                        break;
                    }
                default://дефолтный ответ
                    Console.WriteLine(message.Type);
                    client.SendPhotoAsync(message.Chat.Id, send_default);
                    client.SendAudioAsync(message.Chat.Id, send_default);
                    client.SendAudioAsync(message.Chat.Id, send_default);

                    break;
            }*/

        }
    }
}
