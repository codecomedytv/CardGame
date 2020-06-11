namespace CardGame.Server.Game.Network.Messages
{
    public class Resolve: Message
    {
        public Resolve()
        {
            Player[Command] = (int) GameEvents.Resolve;
            Opponent[Command] = (int) GameEvents.Resolve;
        }
    }
}