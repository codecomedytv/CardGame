using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class Draw: Event
    {
        public readonly ISource Source;
        public readonly Card Card;
        public readonly Player Controller;

        public Draw(ISource source, Player controller, Card card)
        {
            Identity = GameEvents.Draw;
            Source = source;
            Controller = controller;
            Card = card;
        }
    }
}
