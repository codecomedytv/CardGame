using System.Threading.Tasks;
using CardGame.Client.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class Activate: Command
    {
        private readonly Card Card;
        private readonly int PositionInLink;

        public Activate(Card card, int positionInLink)
        {
            Card = card;
            PositionInLink = positionInLink;
        }
        protected override async Task<object[]> Execute()
        {
            Card.ChainIndex = PositionInLink;
            QueueCallback(Card, 0, nameof(Card.FlipFaceUp));
            QueueCallback(Card, 0, nameof(Card.AddToChain));
            Gfx.Start();
            return await ToSignal(Gfx, "tween_all_completed");
        }
    }
}