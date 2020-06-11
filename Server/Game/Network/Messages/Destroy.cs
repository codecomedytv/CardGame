using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class Destroy: Message
    {
        public Destroy(Card card)
        {
            Player[Command] = (int) GameEvents.CardDestroyed;
            Player[Id] = card.Id;
            Opponent[Command] = (int) GameEvents.OpponentCardDestroyed;
            Opponent[Id] = card.Id;
        }
    }
}
