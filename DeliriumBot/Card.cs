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
        public string Description { get; }
        public string View { get; }
        public Card(string name, string description, string view)
        {
            Name = name;
            Description = description;
            View = view;
        }
    }
}
