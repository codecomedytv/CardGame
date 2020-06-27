using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Zones;

namespace CardGame.Server.Game.Events
{
    public class Move: Event
    {
        public readonly ISource Source;
        public readonly Card Card;
        public readonly Zone Origin;
        public readonly Zone Destination;

        public Move()
        {
        }

        public Move(GameEvents identity, ISource source, Zone origin, Card card, Zone destination)
        {
            GameEvent = identity;
            Source = source;
            Card = card;
            Origin = origin;
            Destination = destination;
        }

    }
}