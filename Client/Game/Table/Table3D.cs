using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game.Table
{
    public class Table3D: Spatial, ITableView
    {
        // 3D Implementation of the Card Game
        private readonly PackedScene Scene;
        private readonly Spatial Instance;
        public IPlayerView PlayerView { get; set; }
        public IPlayerView OpponentView { get; set; }

        public Table3D()
        {
            Scene = (PackedScene) GD.Load("res://Client/Game/Table/Table3D/Table.tscn");
            Instance = (Spatial) Scene.Instance();
            AddChild(Instance);
        }
    }
}