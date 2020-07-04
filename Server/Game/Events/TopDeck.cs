using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class TopDeck: Event
    {
        public readonly ISource Source;
        public readonly Player Owner;
        public readonly Card Card;

        public TopDeck(ISource source, Player owner, Card card)
        {
            Identity = GameEvents.TopDeck;
            Source = source;
            Owner = owner;
            Card = card;
        }
    }
}