using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class ResolveCard: Event
    {
        public readonly ISource Source;
        public readonly Card Card;

        public ResolveCard(Card card)
        {
            Source = card;
            Card = card;
        }

        public override void SendMessage(Message message)
        {
            message(Card.Controller.Id, "ResolveCard", Card.Id);
            message(Card.Opponent.Id, "ResolveCard", Card.Id);
        }
    }
}