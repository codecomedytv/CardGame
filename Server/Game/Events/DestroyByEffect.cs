using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Network;

namespace CardGame.Server.Game.Events
{
    public class DestroyByEffect : Event
    {
        public readonly ISource Source;
        public readonly Player Owner;
        public readonly Card Card;

        public DestroyByEffect(ISource source, Player owner, Card card)
        {
            Identity = GameEvents.DestroyByEffect;
            Source = source;
            Owner = owner;
            Card = card;
        }
    }
}