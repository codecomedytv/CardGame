namespace CardGame.Server.Game.Commands
{
    public class GameOver : Command
    {
        public readonly ISource Source;
        public readonly Player Winner;
        public readonly Player Loser;

        public GameOver(Player winner, Player loser)
        {
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