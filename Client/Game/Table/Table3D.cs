using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game.Table
{
    public class Table3D: Spatial, ITableView
    {
        private static int _instanceCount = 0;
        public IPlayerView PlayerView { get; }
        public IPlayerView OpponentView { get; }
        
        public Table3D()
        {
            
            var scene = (PackedScene) GD.Load("res://Client/Game/Table/Table3D/Table.tscn");
            var instance = (Spatial) scene.Instance();
            AddChild(instance);
            if (_instanceCount > 0)
            {
               instance.Translation = new Vector3(0, -20, 0);
            }
            PlayerView = (IPlayerView) instance.GetNode("PlayMat/Player");
            OpponentView = (IPlayerView) instance.GetNode("PlayMat/Opponent");
            _instanceCount += 1;
        }

    }
}