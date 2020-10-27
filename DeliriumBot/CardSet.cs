using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliriumBot
{
    internal static class CardSet
    {
        private static Dictionary<Card, int> DefaultAmounts { get; set; }
        private static List<Card> DefaultCards { get; set; }
        private static void Load()
        {
            DefaultAmounts = new Dictionary<Card, int>();
            DefaultCards = new List<Card>();
            var file = Strings.Cards;
            var lines = Strings.Cards.Split('\n');
            foreach (var line in lines)
            {
                var splitted = line.Split(';');
                var cardName = splitted[0];
                var cardDescription = splitted[1];
                var defaultCardAmount = Convert.ToInt32(splitted[2]);

                var card = new Card(cardName, cardDescription);

                DefaultCards.Add(card);
                DefaultAmounts.Add(card, defaultCardAmount);
            }
        }
        private static List<Card> FillCardSet(List<Card> cardSet, Card fillingCard, int multiplier = 1)
        {
            for (var i = 0; i < DefaultAmounts[fillingCard] * multiplier; i++)
            {
                cardSet.Add(fillingCard);
            }
            return cardSet;
        }

        private static IEnumerable<T> Shuffle<T>(List<T> list)
        {
            var random = new Random();
            for (var i = list.Count() - 1; i > 1; i--)
            {
                var r = random.Next(i + 1);
                var temp = list[r];
                list[r] = list[i];
                list[i] = temp;
            }
            return list;
        }

        public static IEnumerable<Card> GetCardSet()
        {
            Load();
            return DefaultCards;
        }

        public static IEnumerable<Card> GetShuffledCardSet(IEnumerable<Card> cardSet, int mulptiplier = 1)
        {
            var result = new List<Card>(64);
            result = cardSet.Aggregate(result, (current, card) => FillCardSet(current, card, mulptiplier));
            return Shuffle(result);
        }
    }
}
