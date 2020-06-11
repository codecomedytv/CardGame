using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class SetSupport: Message
    {
        public SetSupport(Card card)
        {
            Player[Command] = (int) GameEvents.SetFaceDown;
            Player[Id] = card.Id;
            Opponent[Command] = (int) GameEvents.OpponentSetFaceDown;
        }
    }
}
