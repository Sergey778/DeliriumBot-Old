using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliriumBot
{
    class Card
    {
        public string Name { get; private set; }
        public int Amount { get; set; }
        public string Describtion { get; private set; }
        public Card(string name, int amount, string desc)
        {
            Name = name;
            Amount = amount;
            Describtion = desc;
        }
    }
    class Program
    {
        public const string DBotToken = "247668756:AAHUvgkNcAEM_cVFtVGZtqwyNsig4s6WO70";
        static Telegram.Bot.TelegramBotClient bot;
        public static int PreviousCard;
        static void Main(string[] args)
        {
            InitializeCardSet();
            bot = new Telegram.Bot.TelegramBotClient(DBotToken);
            bot.OnMessage += OnReceiveMessage;
            bot.StartReceiving();
            while (true) { }
        }

        static void OnReceiveMessage(object sender, Telegram.Bot.Args.MessageEventArgs args)
        {
            if (args.Message.Text == "?")
            {
                bot.SendTextMessageAsync(args.Message.Chat.Id, cards[PreviousCard].Describtion);
            }
            else if (args.Message.Text.ToUpper() == "RSETART" || args.Message.Text.ToUpper() == "NEWGAME")
            {
                InitializeCardSet();
            }
            else
            {
                bot.SendTextMessageAsync(args.Message.Chat.Id, NextCard());
            }
        }

        static List<Card> cards = new List<Card>();

        public static void InitializeCardSet()
        {
            cards.Add(new Card("Я", 2, "Этот бокал для тебя"));
            cards.Add(new Card("Ты", 2, "Этот бокал передайа другу"));
            cards.Add(new Card("Леди", 3, "Все дамы, присутствующие за столом пьют"));
            cards.Add(new Card("Джентельмены", 3, "Когда выпадает карта \"Джентельмены\", все мущины присутствующие за столом пьют."));
            cards.Add(new Card("Тост", 3, "Произносишь тост, все игроки выпивают."));
            cards.Add(new Card("Передай налево", 3, "Игрок, сидящий слева от тебя пьет"));
            cards.Add(new Card("Передай направо", 3, "Игрок, сидящий справа от тебя пьет"));
            cards.Add(new Card("Вызов", 1, "Выбираешь игрока, которыйы пьет вместе с тобой"));
            cards.Add(new Card("Все на пол", 2, "Все игроки должны коснуться пола. Последний коснувшийся пола пьет."));
            cards.Add(new Card("Прозвище", 2, "Придумываешь прозвище любому игроку. Тот, кто обратился к игроку по имени или как-то иначе, пьет. Прозвище закрепляется до конца игры."));
            cards.Add(new Card("Твои правила", 3, "Стновишься Rulemaster'ом и придумываешь \"свои правила\". Следующий Rulemaster отменяет праила предыдущего и придумавает свои"));
            cards.Add(new Card("Секретная служба", 2 ,"Когда выпадает карта \"Секретная служба\", все игроки прикладывают ладонь к уху, изображая телохранителей. Кто последний поднес, становиться президентом и пьет."));
            cards.Add(new Card("Раздень друга", 2, "Выбираешь, кто из игроков снимает с себя какой-нибудь предмет одежды. Затем остальные игроки сообща выбирают, что он снимает."));
            cards.Add(new Card("Брудершафт", 2, "Выбираешь, с кем ты хочешь выпить на брудершафт"));
            cards.Add(new Card("Шах и мат", 2, "Выбираешь игрока, который будет пить с тобой на протяжении всей игры каждый раз, когда ты сделаешь ошибку."));
            cards.Add(new Card("Повтори за мной", 2, "Просишь игрока повторить за тобой (скороговорку или сложнопроизносимое слово). Если у него не получается - он пьет. Если получилось - пьешь ты."));
            cards.Add(new Card("Неудобные вопросы", 3, "Каждый игрок имеет право задать тебе вопрос. Любой вопрос. Если ты отказываешься отвечать - ты пьешь."));
            cards.Add(new Card("Смена  мест", 1, "Игроки меняются местами."));
            cards.Add(new Card("СМС", 2, "Вытянувший карту берет телефон у любого из игроков и отправляет на люой контакт з записной книжки СМС. Если на сообщение ответили, то все игроки придумавают ответ."));
            cards.Add(new Card("Нос", 2, "Все игроки должны коснуться носа. Последний, коснувшийся носа, пьет."));
            cards.Add(new Card("Смена одежды", 1, "Игроки меняются одеждой с соседом слева"));
            cards.Add(new Card("Категория", 2, "Вытянувший карту придумывает категорию (марки презервативов, музыкальные группы 90х годов, фильмы с Арнольдом Шварценеггером и т.д.). Остальные игроки называют слова соответствующие данной категории. Кто не сможет - пьет."));
            cards.Add(new Card("Я никогда НЕ", 3, "Говоришь, то что ты \"Никогда не делал\" (но на самом деле делал или очень хотел бы). Тот, кто это делал - пьет. Можно придумывать что-нибудь на  интимные темы например: \"Я никогда не имитировала оргазм\""));
            cards.Add(new Card("Вопросы", 2, "Игрок задает вопрос игроку слева. Отвечать на него не нужно, надо быстро задать вопрос соседу. Сбился? ошибся? Запнулся? - Пьешь!"));
            cards.Add(new Card("Цвет", 2, "Игрок называет цвет, следующий повторяет его и называет свой и так далее. Кто сбился - пьет."));
            cards.Add(new Card("Бармен", 1, "Этот человек следит за тем, чтобы у всех всегда был алкоголь. Он же решает спорные вопросы в игре, а также следит за тем, чтобы игроки не свалились под стол."));
            cards.Add(new Card("Кубок" , 4, "Первыве три игрока, которые вытаскивают эту карту, сливают свой алкоголь в пустой бокал на столе. Четвертый игрок, который вытянул эту карту выпивает этот бокал."));
            cards.Add(new Card("Саймон говорит", 1, "Тот, кто вытащил эту карту, делает какой-нибудь жест (показывает кукиш, трогает ухо), следующий делает тоже самое и добавляет свой. Третий игрок выполняет все три и добавляет свой. И так до тех пор пока кто-то не ошибется."));
            cards.Add(new Card("Аллитерация", 2, "Игрок, вытащивший эту карту, придумавает слово (существительное), следующий игрок придумаывает слово на ту же первую букву, чтобы получилось предолжение или словосочетание. И так далее пока кто-то не собьется. Союзы и междометья могут присутствовать."));
            cards.Add(new Card("Товарищ за@!?л", 1, "Товарищ (прапорщик) За@!?л - самая подлая карта. Когда игрок вытаскивает её, он получает правонадоедать всем своими вопросами. Нельзя отвечать на эти вопросы. Если вы замечатлись и ответили на его вопрос - вы пьете. Отправить товарища в отстаку может только Rulemaster"));
        }
        static string NextCard()
        {
            int[] CardDrow = new int[cards.Count];
            CardDrow[0] = cards[0].Amount;
            for (int i = 1; i < cards.Count; i++)
            {
                CardDrow[i] = cards[i].Amount + CardDrow[i - 1];
            }
            Random card = new Random();
            int CurrentCard = card.Next(1, CardDrow[CardDrow.Length - 1]);
            for(int i = 0; i < CardDrow.Length; i++)
            {
                if(CardDrow[i] >= CurrentCard)
                {
                    cards[i].Amount -= 1;
                    PreviousCard = i;
                    return cards[i].Name;
                }
            }
            return cards[PreviousCard].Name;
        }
    }
}
