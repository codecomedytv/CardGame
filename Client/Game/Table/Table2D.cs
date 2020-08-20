using CardGame.Client.Game.Players;

namespace CardGame.Client.Game.Table
{
    public class Table2D: ITableView
    {
        // 2D Implementation Of The Card Game
        public IPlayerView PlayerView { get; set; }
        public IPlayerView OpponentView { get; set; }
    }
}