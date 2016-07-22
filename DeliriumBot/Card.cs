using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliriumBot
{
    class Card
    {
        public string Name { get; }
        public int Amount { get; }
        public string Description { get; }
        public Card(string name, int amount, string description)
        {
            Name = name;
            Amount = amount;
            Description = description;
        }
    }
}
