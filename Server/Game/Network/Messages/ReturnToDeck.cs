using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class ReturnToDeck: Message
    {
        public ReturnToDeck(Card card)
        {
            Player[Command] = (int)GameEvents.ReturnToDeck;
            Player[Id] = card.Id;
            Opponent[Command] = (int) GameEvents.OpponentReturnedToDeck;
            Opponent[Id] = card.Id;
        }
    }
}