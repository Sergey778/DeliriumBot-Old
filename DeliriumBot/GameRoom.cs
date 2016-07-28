using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace DeliriumBot
{
    class GameRoom
    {
        public long ChatId { get; }
        public IEnumerable<Card> CardSet { get; private set; } = new List<Card>();
        public Card PreviousCard { get; private set; } = null;

        public GameRoom(long chatId)
        {
            ChatId = chatId;
        }

        public void HandleMessage(TelegramBotClient client, MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.TextMessage)
            {
                HandleText(client, e.Message.Text);
            }
        }

        public void HandleText(TelegramBotClient client, string text)
        {
            var commands = text.Split(' ');
            var newGameCommands = new List<string>
            {
                "/start", "/newgame", "/restart"
            };
            if (newGameCommands.Exists(x => commands[0] == x))
            {
                PreviousCard = null;
                CardSet = DeliriumBot.CardSet.GetShuffledCardSet(DeliriumBot.CardSet.GetCardSet(), GetCardSetCount(commands));
                client.SendTextMessageAsync(ChatId, Strings.NewGameStart);
            }
            else if (CardSet.Count() == 0)
            {
                client.SendTextMessageAsync(ChatId, Strings.SetEnded);
            }
            else if (commands[0] == "?")
            {
                client.SendTextMessageAsync(ChatId, PreviousCard == null ? Strings.NoPreviousCard : PreviousCard.Description);
            }
            else if (commands[0] == "/count")
            {
                client.SendTextMessageAsync(ChatId, CardSet.Count().ToString());
            }
            else if (commands[0] == "/bancard")
            {
                var cardName = string.Concat(text.SkipWhile(x => x != ' ').Skip(1)).Trim();
                CardSet = CardSet.Where(card => card.Name != cardName);
                client.SendTextMessageAsync(ChatId, "Карты были убраны из колоды");
            }
            else
            {
                PreviousCard = CardSet.First();
                CardSet = CardSet.Skip(1);
                client.SendTextMessageAsync(ChatId, PreviousCard.Name);
            }
        }

        private int GetCardSetCount(IEnumerable<string> commands)
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
