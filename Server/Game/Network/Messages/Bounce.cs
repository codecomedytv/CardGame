using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
{
    public class Bounce: Message
    {
        public Bounce(Card card)
        {
            // TODO: Figure This Out
            // Player Bounce inherently thinks its bouncing to his own hand
            // but this won't work properly if bouncing an opponents card
            // We could add a check clientside for the owner OR we could
            // use a dedicated move-command like we do serverside (which would reduce
            // quite a bit of code but client & serverside)
            Player[Command] = (int) GameEvents.Bounce;
            Player[Id] = card.Id;
            Opponent[Command] = (int) GameEvents.OpponentBounce;
            Opponent[Id] = card.Id;
        }
    }
}