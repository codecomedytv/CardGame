using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class Bounce: Message
    {
        public Bounce(Card card)
        {
            Player[Command] = (int) GameEvents.Bounce;
            Player[Id] = card.Id;
            Opponent[Command] = (int) GameEvents.OpponentBounce;
            Opponent[Id] = card.Id;
        }
    }
}