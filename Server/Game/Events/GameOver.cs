namespace CardGame.Server.Game.Events
{
    public class GameOver : Event
    {
        public readonly ISource Source;
        public readonly Player Winner;
        public readonly Player Loser;

        public GameOver(Player winner, Player loser)
        {
            GameEvent = GameEvents.GameOver;
            Source = winner;
            Winner = winner;
            Loser = loser;
        }
        
    }
}