using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class Discard: Event
    {
        public readonly ISource Source;
        public readonly Player Owner;
        public readonly Card Card;

        public Discard(ISource source, Player owner, Card card)
        {
            Identity = GameEvents.Discard;
            Source = source;
            Owner = owner;
            Card = card;
        }
    }
}