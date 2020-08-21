using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game.Table
{
    public class Table3D: ITableView
    {
        // 3D Implementation of the Card Game
        public IPlayerView PlayerView { get; set; }
        public IPlayerView OpponentView { get; set; }
    }
}