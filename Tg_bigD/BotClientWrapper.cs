using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Timers;
using System.IO;


namespace Tg_bigD
{
    class BaseBotClientWrapper
    {
        Timer timer = new Timer();
        Timer timer2 = new Timer();
        object locker = new object();
        List<Message> MessagesStorage = new List<Message>();
        string token;
        TelegramBotClient client;



        public BaseBotClientWrapper(string token)
        {
            this.token = token;
            client = new TelegramBotClient(token);
            client.OnUpdate += BotOnUpdateReceived;
            client.OnMessage += BotOnMessageReceived;
            timer.Interval = 300;
            timer.Elapsed += AlbumHandler;
            client.OnMessageEdited += BotOnMessageEdited; 
            
        }



        public void Start()
        {
            client.StartReceiving();
        }


        private void AlbumHandler(object sender, ElapsedEventArgs e)
        {
            lock (locker)
            {
                
                if (MessagesStorage.Count != 0)
                {
                    List<InputMediaPhoto> temp = new List<InputMediaPhoto>();
                    
                    foreach (Message mess in MessagesStorage)
                    {
                        InputMediaPhoto inputMediaPhoto = new InputMediaPhoto(new InputMedia(mess.Photo[0].FileId));
                        temp.Add(inputMediaPhoto);
                    }
                    client.SendMediaGroupAsync(temp, MessagesStorage[0].Chat.Id);
                    MessagesStorage.Clear();
                }
            }

        }

        public virtual void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs) /// метод
        {
            
            Message message = messageEventArgs.Message;
            string userFirstName = message.From.FirstName;
            
            if (message.MediaGroupId != null)
            {
                MessagesStorage.Add(message);
                timer.Start();
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                if(message.Text == "/start")
                {
                    client.SendTextMessageAsync(message.Chat.Id, $"Привет, {userFirstName}!");  
                }
                else
                {
                    client.SendTextMessageAsync(message.Chat.Id, message.Text);
                }
                       
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Audio)
            {
                client.SendAudioAsync(message.Chat.Id, message.Audio.FileId, message.Caption);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Contact)
            {
                client.SendContactAsync(message.Chat.Id, message.Contact.PhoneNumber, message.Contact.FirstName, message.Contact.LastName);

            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
            {
                client.SendPhotoAsync(message.Chat.Id, message.Photo[0].FileId, message.Caption);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Location)
            {
                client.SendLocationAsync(message.Chat.Id, message.Location.Latitude, message.Location.Longitude);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                client.SendDocumentAsync(message.Chat.Id, message.Document.FileId, message.Caption);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Animation)
            {
                client.SendAnimationAsync(message.Chat.Id, message.Animation.FileId);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Sticker)
            {
                client.SendDocumentAsync(message.Chat.Id, message.Sticker.FileId, message.Caption);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.VideoNote)
            {
                client.SendVideoNoteAsync(message.Chat.Id, message.VideoNote.FileId);
            }
            else if (message.Type == Telegram.Bot.Types.Enums.MessageType.Video)
            {
                client.SendVideoAsync(message.Chat.Id, message.Video.FileId,0 ,0 ,0 , message.Caption);
            }
        }

        /* public virtual void BotOnUpdateReceived(object sender, UpdateEventArgs e)
         {

             Message message1 = e.Update.EditedMessage;
             string chatIdUpdated = message1.Chat.Id.ToString();
             if (message1 == null)
             {

             }
                 if (chatIdUpdated.Contains("-") == true)
             {
                 string path = @"D:\chatsIdList.txt";
                 FileInfo fileInf = new FileInfo(path);
                 if (fileInf.Exists == true)
                 {
                     Console.WriteLine(fileInf.Exists);

                     using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                     {
                         sw.WriteLine($"{chatIdUpdated}\n");
                         sw.Close();

                     }
                     using (StreamReader sr = new StreamReader(path))
                     {
                         Console.WriteLine(sr.ReadToEnd());
                         string str = sr.ReadToEnd();
                         //проверить файл на дубли
                         //записать в конец файла
                     }
                 }
             }


             if (messageEdited == null) return;*/
          
        public void BotOnMessageEdited(object sender, MessageEventArgs e)
        {
            var chatId = e.Message.Chat.Id;
            var newmember = e.Message.NewChatMembers;
            Console.WriteLine($"Сработало событие При изменении сообщения/ Ади чата {chatId}");
            client.SendTextMessageAsync(chatId, "😎 Зачем ты меняешь текст? ");
        }


        public void BotOnUpdateReceived(object sender, UpdateEventArgs e)
        {
            try
            {
                if (e.Update.Message == null)
                {
                    Console.WriteLine("Сообщение пустое");
                }
                else if (e.Update.Message.Type.ToString() == "ChatMembersAdded")
                {
                    User[] newChatMember = e.Update.Message.NewChatMembers;
                    var isBot = newChatMember[0].IsBot;
                    Console.WriteLine($" Сработал метод обновлений с параметрами \n" +
                   $"Статус новый пользователь: {newChatMember[0].Username}\n" +
                   $"Это бот?: {isBot}");
                    
                    client.SendTextMessageAsync(e.Update.Message.Chat.Id, $"Приветствуем {e.Update.Message.NewChatMembers[0].FirstName}  {e.Update.Message.NewChatMembers[0].LastName} в нашем тестовом чате!😀😀😀");
                }
                else if (e.Update.Message.Type.ToString() == "ChatMemberLeft")
                {
                    Console.WriteLine($"Пользователь {e.Update.Message.Type.ToString()} вышел из группы");
                }
                else if (e.Update.Message.Type.ToString() == "MessagePinned")
                {
                    
                    client.SendTextMessageAsync(e.Update.Message.Chat.Id, $"{e.Update.Message.From.Username}   этом чате закреплять сообщения запрещено!🤣🤣🤣");
                    System.Threading.Thread.Sleep(3000);
                    client.UnpinChatMessageAsync(e.Update.Message.Chat.Id);
                    Console.WriteLine("Действие таймера окончено");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    
}
