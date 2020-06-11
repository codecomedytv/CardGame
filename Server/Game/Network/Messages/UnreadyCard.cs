using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class UnreadyCard: Message
    {
        public UnreadyCard(Card card)
        {
            Player[Command] = (int) GameEvents.UnreadyCard;
            Player[Id] = card.Id;
        }
    }
}