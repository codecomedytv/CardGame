using System.Threading.Tasks;
using CardGame.Client.Cards;

namespace CardGame.Client.Room.Commands
{
    public class BounceCard: Command
    {
        public readonly Player Owner;
        public readonly Card Bounced;
        public bool IsOpponent;

        public BounceCard(Player owner, Card bounced, bool isOpponent)
        {
            Owner = owner;
            Bounced = bounced;
            IsOpponent = isOpponent;
        }

        protected override async Task<object[]> Execute()
        {
            MoveCard(Bounced, Owner.Hand);
            if (IsOpponent) { QueueCallback(Bounced, 0.1F, nameof(Card.FlipFaceDown)); }
            return await Start();
        }
    }
}
