using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class DestroyByBattle: Event
    {
        public readonly ISource Source;
        public readonly Player Owner;
        public readonly Card Card;

        public DestroyByBattle(ISource source, Player owner, Card card)
        {
            Identity = GameEvents.DestroyByBattle;
            Source = source;
            Owner = owner;
            Card = card;
        }
    }
}