using CardGame.Server.Room.Cards;

namespace CardGame.Server.Room.Network.Messages
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
