using System.Threading.Tasks;
using CardGame.Client.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class Trigger: Command
    {
        private readonly Card Card;
        private readonly int PositionInLink;
        public Trigger(Card card, int positionInLink)
        {
            Card = card;
            PositionInLink = positionInLink;
        }
        
        protected override async Task<object[]> Execute()
        {
            QueueCallback(Card, 0.1F, nameof(Card.AddToChain));
            return await Start();
        }
    }
}