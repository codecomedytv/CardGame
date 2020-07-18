#nullable enable
using System.Threading.Tasks;
using CardGame.Client.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class Activate: Command
    {
        private readonly Card Card;
        private readonly int PositionInLink;
        private readonly Card? Target;

        public Activate(Card card, int positionInLink, Card? target)
        {
            Card = card;
            PositionInLink = positionInLink;
            Target = target;
        }
        protected override async Task<object[]> Execute()
        {
            Card.ChainIndex = PositionInLink;
            QueueCallback(Card, 0, nameof(Card.FlipFaceUp));
            QueueCallback(Card, 0, nameof(Card.AddToChain));
            if (Target != null)
            {
                QueueCallback(Target, 0, nameof(Card.ShowAsTargeted));
            }
            return await Start();
        }
    }
}