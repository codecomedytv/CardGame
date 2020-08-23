using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game.Table
{
    public class Table3D: Spatial, ITableView
    {
        public IPlayerView PlayerView { get; }
        public IPlayerView OpponentView { get; }
        
        public Table3D()
        {
            var scene = (PackedScene) GD.Load("res://Client/Game/Table/Table3D/Table.tscn");
            var instance = (Spatial) scene.Instance();
            AddChild(instance);
            PlayerView = (IPlayerView) instance.GetNode("PlayMat/Player");
            OpponentView = (IPlayerView) instance.GetNode("PlayMat/Opponent");
        }

    }
}