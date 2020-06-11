using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class AutoTarget: Message
    {
        public AutoTarget(Card card)
        {
            Player[Command] = (int) GameEvents.AutoTarget;
            Player[Id] = card.Id;
        }
        
    }
}