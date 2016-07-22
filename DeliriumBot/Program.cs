using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliriumBot
{
    class Program
    {
        private static string DBotToken { get; } = DeliriumBot.Resources.BotToken;
        private static Telegram.Bot.TelegramBotClient bot;

        private static Dictionary<long, GameRoom> rooms = new Dictionary<long, GameRoom>();
        static void Main(string[] args)
        {
            bot = new Telegram.Bot.TelegramBotClient(DBotToken);
            bot.OnMessage += OnReceiveMessage;
            bot.StartReceiving();
            while (true) { }
        }

        private static void OnReceiveMessage(object sender, Telegram.Bot.Args.MessageEventArgs args)
        {
            var id = args.Message.Chat.Id;
            if (rooms.ContainsKey(id))
            {
                rooms[id].HandleMessage(bot, args);
            }
            else
            {
                var room = new GameRoom(id);
                room.HandleMessage(bot, args);
                rooms.Add(id, room);
            }
        }
    }
}
