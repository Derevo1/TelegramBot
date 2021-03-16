using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace ConsoleApp1
{
    class Program
    {
        static ITelegramBotClient botClient;
        static void Main(string[] args)
        {                        
            botClient = new TelegramBotClient("1610712214:AAFV1hp5M6Mk5XzKE2jZcsLvHokC1FrkL7w"); //API токен
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine( $"User {me.Id} Bot name {me.FirstName}.  \n@{me.Username} "); //Информация чата

            botClient.StartReceiving();       
            botClient.OnMessage += Сommand; //Ответ на команду 

            Console.ReadKey();

        }

        static string Request(string input)
        {
            string result,coll="";
            string url = $"https://archive.org/metadata/{input}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
           HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                result = stream.ReadToEnd();
            }
            Book res = JsonConvert.DeserializeObject<Book>(result);
            foreach (var file in res.Files)
            {
                coll = Convert.ToString(file.name);
            }
            return coll;
        }
        static async void bookCommand(object sender, MessageEventArgs e)
        {
            
            if(e.Message.Text == "Prince")
            {
                await botClient.SendTextMessageAsync(e.Message.Chat, text: $"{Request("principleofrelat00eins")}");
            }
        }
        static async void audioCommand(object sender, MessageEventArgs e)
        {
            if(e.Message.Text == "Серега")
            await botClient.SendAudioAsync(e.Message.Chat, "https://zaycev.net/musicset/dl/859c3fa89d1dc037a4a88c691e931cdc/11353239.json?spa=false");
        }
        static async void Сommand(object sender, MessageEventArgs e)
        {
            var me = botClient.GetMeAsync().Result;
            switch (e.Message.Text)
            {
                case "/say":
                Console.WriteLine("Ответ на команду /say");
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: $"Hello my name is *{me.FirstName}*", parseMode: ParseMode.Markdown);
                break;
                case "/leave":
                Console.WriteLine("Ответ на команду /leave");
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "*BYE*", parseMode: ParseMode.Markdown);
                break;
                case "/audio":
                Console.WriteLine("Ответ на команду /audio");
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "*Введите название*", parseMode: ParseMode.Markdown);
                botClient.OnMessage += audioCommand;
                break;
                case "/book":
                Console.WriteLine("Ответ на команду /book");
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "*Введите название*", parseMode: ParseMode.Markdown);
                botClient.OnMessage += bookCommand;
                break;
                case "/downl":
                Console.WriteLine("Ответ на команду /downl");
                await botClient.SendVideoAsync(chatId: e.Message.Chat, video: "https://archive.org/download/popeye_taxi-turvey/popeye_taxi-turvey.gif", thumb: "https://archive.org/download/popeye_taxi-turvey/popeye_taxi-turvey.gif");
                //await botClient.SendDocumentAsync(chatId: e.Message.Chat, caption:"Black Boy", document: "https://ru.pdfdrive.com/download.pdf?id=189353487&h=146465099d6e8371a4c9e734e415d94e&u=cache&ext=pdf");
                break;
                case "/help":
                Console.WriteLine("Ответ на команду /help");
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Список команд:\n*/say*\n*/leave*\n/help", parseMode: ParseMode.Markdown);
                break;
            }
        }

    }
}
