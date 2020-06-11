namespace CardGame.Server.Game.Network.Messages
{
    public class LoadDeck: Message
    {
        public LoadDeck(int loadedCount)
        {
            Player[Command] = (int) GameEvents.LoadDeck;
            Player["count"] = loadedCount;
            Opponent[Command] = (int) GameEvents.OpponentLoadDeck;
            Opponent["count"] = loadedCount;
        }
    }
}