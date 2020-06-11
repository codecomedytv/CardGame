using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class Mill: Message
    {
        public Mill(Card card)
        {
            Player[Command] = (int)GameEvents.Mill;
            Player[Id] = card.Id;
            Player[SetCode] = (int)card.SetCode;
            Opponent[Command] = (int) GameEvents.OpponentMill;
            Opponent[Id] = card.Id;
            Opponent[SetCode] = (int) card.SetCode;

        }
    }
}
