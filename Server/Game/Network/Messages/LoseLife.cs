namespace CardGame.Server.Game.Network.Messages
{
    public class LoseLife: Message
    {
        public LoseLife(int lifeLost)
        {
            Player[Command] = (int) GameEvents.LoseLife;
            Player["lifeLost"] = lifeLost;
            Opponent[Command] = (int) GameEvents.OpponentLoseLife;
            Opponent["lifeLost"] = lifeLost;
        }
    }
}