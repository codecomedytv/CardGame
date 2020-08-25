using System.Collections.Generic;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class Table: Spatial
    {
       // private static int _instanceCount = 0;
        public IPlayer PlayerView { get; }
        public IPlayer OpponentView { get; }
        
        public Table()
        {
            
            var scene = (PackedScene) GD.Load("res://Client/Game/Table.tscn");
            var instance = (Spatial) scene.Instance();
            AddChild(instance);
            //if (_instanceCount > 0)
           // {
               //instance.Translation = new Vector3(0, -20, 0);
               //instance.Visible = false;
            //}
            PlayerView = (IPlayer) instance.GetNode("PlayMat/Player");
            OpponentView = (IPlayer) instance.GetNode("PlayMat/Opponent");
            //_instanceCount += 1;
        }
    }
}