using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class SetAsAttacker: Message
    {
        public SetAsAttacker(Card card)
        {
            Player[Command] = (int) GameEvents.SetAsAttacker;
            Player[Id] = card.Id;
        }
    }
}