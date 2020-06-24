using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Zones;

namespace CardGame.Server.Game.Commands
{
    public class Move: Command
    {
        public GameEvents Identity;
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

        public override void Execute()
        {
            Origin.Remove(Card);
            Destination.Add(Card);
            Card.Zone = Destination;
        }

        public override void Undo()
        {
            Destination.Remove(Card);
            Origin.Add(Card);
            Card.Zone = Destination;
        }
    }
}