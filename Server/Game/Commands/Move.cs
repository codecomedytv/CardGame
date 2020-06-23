using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
using CardGame.Server.Game.Zones;

namespace CardGame.Server.Game.Commands
{
    public class Move: GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Card Card;
        public readonly Zone Origin;
        public readonly Zone Destination;

        public Move()
        {
        }

        public Move(ISource source, Card card, Zone destination)
        {
            Source = source;
            Card = card;
            Origin = card.Zone;
            Destination = destination;
        }

        public void Execute()
        {
            Origin.Remove(Card);
            Destination.Add(Card);
            Card.Zone = Destination;
        }

        public void Undo()
        {
            Destination.Remove(Card);
            Origin.Add(Card);
            Card.Zone = Destination;
        }
    }
}