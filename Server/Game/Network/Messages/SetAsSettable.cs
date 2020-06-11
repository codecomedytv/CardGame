using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class SetAsSettable: Message
    {
        public SetAsSettable(Card card)
        {
            Player[Command] = (int) GameEvents.SetSettable;
            Player[Id] = card.Id;
        }
    }
}
