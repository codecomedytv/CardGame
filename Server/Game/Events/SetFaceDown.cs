using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class SetFaceDown: Event
    {
        public readonly ISource Source;
        public readonly Player Controller;
        public readonly Card Card;

        public SetFaceDown(ISource source, Player controller, Card card)
        {
            Identity = GameEvents.SetFaceDown;
            Source = source;
            Controller = controller;
            Card = card;
        }
    }
}