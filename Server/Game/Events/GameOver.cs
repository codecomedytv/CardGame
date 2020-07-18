namespace CardGame.Server.Game.Events
{
    public class GameOver : Event
    {
        public readonly ISource Source;
        public readonly Player Winner;
        public readonly Player Loser;

        public GameOver(Player winner, Player loser)
        {
            Identity = GameEvents.GameOver;
            Source = winner;
            Winner = winner;
            Loser = loser;
        }

        public override void SendMessage(Message message)
        {
            const bool won = true;
            const bool lost = false;
            message(Winner.Id, "GameOver", won);
            message(Loser.Id, "GameOver", lost);
        }
    }
}