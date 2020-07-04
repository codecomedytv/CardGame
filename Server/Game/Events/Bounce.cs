using CardGame.Server.Game.Cards;
using Godot;

namespace CardGame.Server.Game.Events
{
    public class Bounce: Event
    {
        public readonly ISource Source;
        public readonly Player Owner;
        public readonly Card Card;

        public Bounce(ISource source, Player owner, Card card)
        {
            Identity = GameEvents.Bounce;
            Source = source;
            Owner = owner;
            Card = card;
        }
    }
}