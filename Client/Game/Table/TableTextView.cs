using CardGame.Client.Game.Players;

namespace CardGame.Client.Game.Table
{
    public class TableTextView: ITableView
    {
        // Textual Implementation Of The Card Game (no Graphics)
        public IPlayerView PlayerView { get; set; }
        public IPlayerView OpponentView { get; set; }
    }
}