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
        private static IEnumerable<Card> CardSet = GetCardSet();
        private static Card PreviousCard;
        private static IEnumerable<Card> Cards = GetShuffledCardSet(CardSet);
        static void Main(string[] args)
        {
            bot = new Telegram.Bot.TelegramBotClient(DBotToken);
            bot.OnMessage += OnReceiveMessage;
            bot.StartReceiving();
            while (true) { }
        }

        private static void OnReceiveMessage(object sender, Telegram.Bot.Args.MessageEventArgs args)
        {
            if (args.Message.Text == "?" && PreviousCard != null)
            {
                bot.SendTextMessageAsync(args.Message.Chat.Id, PreviousCard.Description);
            }
            else if (args.Message.Text.ToLower() == "/restart" || args.Message.Text.ToLower() == "/newgame")
            {
                PreviousCard = null;
                Cards = GetShuffledCardSet(CardSet);
                bot.SendTextMessageAsync(args.Message.Chat.Id, "Раздача с новой колоды");
            }
            else if (Cards.Count() < 1)
            {
                bot.SendTextMessageAsync(args.Message.Chat.Id, "Колода закончилась. Чтобы начать новую напишите команду '/Restart'");
            }
            else
            {
                var answer = NextCard(Cards);
                PreviousCard = answer.Item1;
                Cards = answer.Item2;
                bot.SendTextMessageAsync(args.Message.Chat.Id, answer.Item1.Name);
            }
        }

        private static List<Card> FillCardSet(List<Card> cardSet, Card fillingCard)
        {
            for (int i = 0; i < fillingCard.Amount; i++)
            {
                cardSet.Add(fillingCard);
            }
            return cardSet;
        }

        private static IEnumerable<T> Shuffle<T>(List<T> list)
        {
            var random = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        private static IEnumerable<Card> GetShuffledCardSet(IEnumerable<Card> cardSet)
        {
            var result = new List<Card>(64);
            foreach (var card in cardSet)
            {
                result = FillCardSet(result, card);
            }
            return Shuffle(result);
        }
        private static List<Card> GetCardSet()
        {
            return new List<Card>
            {
                new Card("Я", 2, "Этот бокал для тебя"),
                new Card("Ты", 2, "Этот бокал передайа другу"),
                new Card("Леди", 3, "Все дамы, присутствующие за столом пьют"),
                new Card("Джентельмены", 3, "Когда выпадает карта \"Джентельмены\", все мущины присутствующие за столом пьют."),
                new Card("Тост", 3, "Произносишь тост, все игроки выпивают."),
                new Card("Передай налево", 3, "Игрок, сидящий слева от тебя пьет"),
                new Card("Передай направо", 3, "Игрок, сидящий справа от тебя пьет"),
                new Card("Вызов", 1, "Выбираешь игрока, которыйы пьет вместе с тобой"),
                new Card("Все на пол", 2, "Все игроки должны коснуться пола. Последний коснувшийся пола пьет."),
                new Card("Прозвище", 2, "Придумываешь прозвище любому игроку. Тот, кто обратился к игроку по имени или как-то иначе, пьет. Прозвище закрепляется до конца игры."),
                new Card("Твои правила", 3, "Стновишься Rulemaster'ом и придумываешь \"свои правила\". Следующий Rulemaster отменяет праила предыдущего и придумавает свои"),
                new Card("Секретная служба", 2 ,"Когда выпадает карта \"Секретная служба\", все игроки прикладывают ладонь к уху, изображая телохранителей. Кто последний поднес, становиться президентом и пьет."),
                new Card("Раздень друга", 2, "Выбираешь, кто из игроков снимает с себя какой-нибудь предмет одежды. Затем остальные игроки сообща выбирают, что он снимает."),
                new Card("Брудершафт", 2, "Выбираешь, с кем ты хочешь выпить на брудершафт"),
                new Card("Шах и мат", 2, "Выбираешь игрока, который будет пить с тобой на протяжении всей игры каждый раз, когда ты сделаешь ошибку."),
                new Card("Повтори за мной", 2, "Просишь игрока повторить за тобой (скороговорку или сложнопроизносимое слово). Если у него не получается - он пьет. Если получилось - пьешь ты."),
                new Card("Неудобные вопросы", 3, "Каждый игрок имеет право задать тебе вопрос. Любой вопрос. Если ты отказываешься отвечать - ты пьешь."),
                new Card("Смена  мест", 1, "Игроки меняются местами."),
                new Card("СМС", 2, "Вытянувший карту берет телефон у любого из игроков и отправляет на люой контакт з записной книжки СМС. Если на сообщение ответили, то все игроки придумавают ответ."),
                new Card("Нос", 2, "Все игроки должны коснуться носа. Последний, коснувшийся носа, пьет."),
                new Card("Смена одежды", 1, "Игроки меняются одеждой с соседом слева"),
                new Card("Категория", 2, "Вытянувший карту придумывает категорию (марки презервативов, музыкальные группы 90х годов, фильмы с Арнольдом Шварценеггером и т.д.). Остальные игроки называют слова соответствующие данной категории. Кто не сможет - пьет."),
                new Card("Я никогда НЕ", 3, "Говоришь, то что ты \"Никогда не делал\" (но на самом деле делал или очень хотел бы). Тот, кто это делал - пьет. Можно придумывать что-нибудь на  интимные темы например: \"Я никогда не имитировала оргазм\""),
                new Card("Вопросы", 2, "Игрок задает вопрос игроку слева. Отвечать на него не нужно, надо быстро задать вопрос соседу. Сбился? ошибся? Запнулся? - Пьешь!"),
                new Card("Цвет", 2, "Игрок называет цвет, следующий повторяет его и называет свой и так далее. Кто сбился - пьет."),
                new Card("Бармен", 1, "Этот человек следит за тем, чтобы у всех всегда был алкоголь. Он же решает спорные вопросы в игре, а также следит за тем, чтобы игроки не свалились под стол."),
                new Card("Кубок" , 4, "Первыве три игрока, которые вытаскивают эту карту, сливают свой алкоголь в пустой бокал на столе. Четвертый игрок, который вытянул эту карту выпивает этот бокал."),
                new Card("Саймон говорит", 1, "Тот, кто вытащил эту карту, делает какой-нибудь жест (показывает кукиш, трогает ухо), следующий делает тоже самое и добавляет свой. Третий игрок выполняет все три и добавляет свой. И так до тех пор пока кто-то не ошибется."),
                new Card("Аллитерация", 2, "Игрок, вытащивший эту карту, придумавает слово (существительное), следующий игрок придумаывает слово на ту же первую букву, чтобы получилось предолжение или словосочетание. И так далее пока кто-то не собьется. Союзы и междометья могут присутствовать."),
                new Card("Товарищ за@!?л", 1, "Товарищ (прапорщик) За@!?л - самая подлая карта. Когда игрок вытаскивает её, он получает правонадоедать всем своими вопросами. Нельзя отвечать на эти вопросы. Если вы замечатлись и ответили на его вопрос - вы пьете. Отправить товарища в отстаку может только Rulemaster")
            };

        }
        private static Tuple<Card, IEnumerable<Card>> NextCard(IEnumerable<Card> cardSet)
        {
            return new Tuple<Card, IEnumerable<Card>>(cardSet.First(), cardSet.Skip(1));
        }
    }
}
