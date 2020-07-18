#nullable enable
using System.Threading.Tasks;
using CardGame.Client.Cards;

namespace CardGame.Client.Room.Commands
{
    public class ResolveCard: Command
    {
        public readonly Card Card;
        private readonly Card? Target;

        public ResolveCard(Card card, Card? target)
        {
            Card = card;
            Target = target;
        }
        protected override async Task<object[]> Execute()
        {
            QueueCallback(Card, 0, nameof(Card.RemoveFromChain));
            if (Target != null)
            {
                QueueCallback(Target, 0, nameof(Card.StopShowingAsTargeted));
            }
            return await Start();
        }
    }
}