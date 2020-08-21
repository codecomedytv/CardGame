using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game.Table
{
    public class Table3D: Spatial, ITableView
    {
        // 3D Implementation of the Card Game
        public IPlayerView PlayerView { get; set; }
        public IPlayerView OpponentView { get; set; }
        
        public static ITableView CreateInstance()
        {
            var tableView = (PackedScene) GD.Load("res://Client/Game/Table/Table3D/Table.tscn");
            return (ITableView) tableView.Instance();
        }
    }
}