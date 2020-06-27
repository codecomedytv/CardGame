namespace CardGame.Server.Game.Commands
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

        public override void Execute()
        {
        }

        public override void Undo()
        {
        }
    }
}