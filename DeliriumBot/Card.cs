namespace DeliriumBot
{
    internal class Card
    {
        public Card(string name, string description)
        {
            Name = name;
            Description = description;
            //  View = view;
        }

        public string Name { get; }
        public string Description { get; }
        public string View { get; }
    }
}