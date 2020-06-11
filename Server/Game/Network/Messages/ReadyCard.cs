using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class ReadyCard: Message
    {
        public ReadyCard(Card card)
        {
            Player[Command] = (int) GameEvents.ReadyCard;
            Player[Id] = card.Id;
        }
    }
}