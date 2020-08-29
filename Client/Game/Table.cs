using System.Collections.Generic;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class Table: Spatial
    {
        public IPlayer PlayerView { get; }
        public IPlayer OpponentView { get; }
        
        public Table()
        {
            var scene = (PackedScene) GD.Load("res://Client/Game/Table.tscn");
            var instance = (Spatial) scene.Instance();
            AddChild(instance);
            PlayerView = (IPlayer) instance.GetNode("PlayMat/Player");
            OpponentView = (IPlayer) instance.GetNode("PlayMat/Opponent");
        }
    }
}