using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliriumBot
{
    internal static class Program
    {
       private static string DBotToken { get; } = DeliriumBot.Resources.BotToken;
        private static Telegram.Bot.TelegramBotClient _bot;

        private static readonly Dictionary<long, GameRoom> Rooms = new Dictionary<long, GameRoom>();
        private static void Main(string[] args)
        {
            _bot = new Telegram.Bot.TelegramBotClient(DBotToken);
            _bot.OnMessage += OnReceiveMessage;
            _bot.StartReceiving();
            while (true) { }
        }

        private static void OnReceiveMessage(object sender, Telegram.Bot.Args.MessageEventArgs args)
        {
            var id = args.Message.Chat.Id;
            if (Rooms.ContainsKey(id))
            {
                Rooms[id].HandleMessage(_bot, args);
            }
            else
            {
                var room = new GameRoom(id);
                room.HandleMessage(_bot, args);
                Rooms.Add(id, room);
            }
        }
    }
}
