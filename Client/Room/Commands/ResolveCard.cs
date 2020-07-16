using System.Threading.Tasks;
using CardGame.Client.Cards;

namespace CardGame.Client.Room.Commands
{
    public class ResolveCard: Command
    {
        public readonly Card Card;

        public ResolveCard(Card card)
        {
            Card = card;
        }
        protected override async Task<object[]> Execute()
        {
            QueueCallback(Card, 0, nameof(Card.RemoveFromChain));
            return await Start();
        }
    }
}