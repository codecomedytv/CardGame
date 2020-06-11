using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class Discard: Message
    {
        public Discard(Card card)
        {
            Player[Command] = (int) GameEvents.Discard;
            Player[Id] = card.Id;
            Opponent[Command] = (int) GameEvents.OpponentDiscard;
            Opponent[Id] = card.Id;
            Opponent[SetCode] = (int) card.SetCode;
        }
    }
}
