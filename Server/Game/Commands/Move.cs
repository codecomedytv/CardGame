using System.Collections.Generic;
using CardGame.Server.Room.Cards;

namespace CardGame.Server.Room.Commands
{
    public class Move: GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Card Card;
        public readonly List<Card> Origin;
        public readonly List<Card> Destination;

        public Move()
        {
        }

        public Move(ISource source, Card card, List<Card> destination)
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