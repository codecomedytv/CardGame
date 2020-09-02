using System;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class Table: Spatial
    {
        public IPlayer PlayerView { get; }
        public IPlayer OpponentView { get; }

        public Action PassPlayPressed;
        public Action EndTurnPressed;
        
        public Table()
        {
            Name = "Table";
            var scene = (PackedScene) GD.Load("res://Client/Game/Table.tscn");
            var instance = (Spatial) scene.Instance();
            AddChild(instance);
            PlayerView = (IPlayer) instance.GetNode("PlayMat/Player");
            OpponentView = (IPlayer) instance.GetNode("PlayMat/Opponent");
            GetNode<Button>("Table3D/HUD/EndTurn").Connect("pressed", this, nameof(EndTurnPressed));
            GetNode<Button>("Table3D/HUD/PassPlay").Connect("pressed", this, nameof(PassPlayPressed));
        }
    }
}