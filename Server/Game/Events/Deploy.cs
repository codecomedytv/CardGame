using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class Deploy: Event
    {
        public readonly ISource Source;
        public readonly Card Card;
        public readonly Player Controller;

        public Deploy(ISource source, Player controller, Card card)
        {
            Source = source;
            Card = card;
            Controller = controller;
        }
    }
}