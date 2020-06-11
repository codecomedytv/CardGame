using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class SetAsActivatable: Message
    {
        public SetAsActivatable(Card card)
        {
            Player[Command] = (int) GameEvents.SetActivatable;
            Player[Id] = card.Id;
        }
    }
}