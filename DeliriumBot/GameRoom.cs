using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace DeliriumBot
{
    internal class GameRoom
    {
        public GameRoom(long chatId)
        {
            ChatId = chatId;
        }

        private long ChatId { get; }
        private IEnumerable<Card> CardSet { get; set; } = new List<Card>();
        private Card PreviousCard { get; set; }

        public void HandleMessage(TelegramBotClient client, MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.TextMessage) HandleText(client, e.Message.Text);
        }

        private void HandleText(TelegramBotClient client, string text)
        {
            var commands = text.Split(' ');
            var newGameCommands = new List<string>
            {
                "/start", "/newgame", "/restart"
            };
            if (newGameCommands.Exists(x => commands[0] == x))
            {
                PreviousCard = null;
                CardSet = DeliriumBot.CardSet.GetShuffledCardSet(DeliriumBot.CardSet.GetCardSet(),
                    GetCardSetCount(commands));
                client.SendTextMessageAsync(ChatId, Strings.NewGameStart);
            }
            else if (!CardSet.Any())
            {
                client.SendTextMessageAsync(ChatId, Strings.SetEnded);
            }
            else
            {
                switch (commands[0])
                {
                    case "?":
                        client.SendTextMessageAsync(ChatId,
                            PreviousCard == null ? Strings.NoPreviousCard : PreviousCard.Description);
                        break;
                    case "/count":
                        client.SendTextMessageAsync(ChatId, CardSet.Count().ToString());
                        break;
                    case "/bancard":
                    {
                        var cardName = string.Concat(text.SkipWhile(x => x != ' ').Skip(1)).Trim();
                        CardSet = CardSet.Where(card => card.Name != cardName);
                        client.SendTextMessageAsync(ChatId, "Карты были убраны из колоды");
                        break;
                    }
                    default:
                        PreviousCard = CardSet.First();
                        CardSet = CardSet.Skip(1);
                        client.SendTextMessageAsync(ChatId, PreviousCard.Name);
                        break;
                }
            }
        }

        private static int GetCardSetCount(IEnumerable<string> commands)
        {
            try
            {
                return Convert.ToInt32(commands.ElementAt(1));
            }
            catch
            {
                return 1;
            }
        }
    }
}