using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class Mill: Event
    {
        public readonly ISource Source;
        public readonly Player Controller;
        public readonly Card Card;

        public Mill(ISource source, Player controller, Card card)
        {
            Source = source;
            Controller = controller;
            Card = card;
        }
    }
}