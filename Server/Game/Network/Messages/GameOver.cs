namespace CardGame.Server.Game.Network.Messages
{
    public class GameOver: Message
    {
        public GameOver()
        {
            Player[Command] = (int) GameEvents.Win;
            Opponent[Command] = (int) GameEvents.Lose;
        }
    }
}
