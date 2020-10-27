using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace DeliriumBot
{
    internal static class Program
    {
        private static TelegramBotClient _bot;

        private static readonly Dictionary<long, GameRoom> Rooms = new Dictionary<long, GameRoom>();
        private static string DBotToken { get; } = DeliriumBot.Resources.BotToken;

        private static void Main(string[] args)
        {
            _bot = new TelegramBotClient(DBotToken);
            _bot.OnMessage += OnReceiveMessage;
            _bot.StartReceiving();
            while (true)
            {
            }
        }

        private static void OnReceiveMessage(object sender, MessageEventArgs args)
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