using System;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class Table: Spatial
    {
        public BasePlayer PlayerView { get; }
        public BasePlayer OpponentView { get; }

        public Action PassPlayPressed;
        public Action EndTurnPressed;
        public Label LifeChange;
        
        public Table()
        {
            var scene = (PackedScene) GD.Load("res://Client/Game/Table.tscn");
            var instance = (Spatial) scene.Instance();
            AddChild(instance);
            PlayerView = (BasePlayer) instance.GetNode("PlayMat/Player");
            OpponentView = (BasePlayer) instance.GetNode("PlayMat/Opponent");
            GetNode<Button>("Table3D/HUD/EndTurn").Connect("pressed", this, nameof(OnEndTurnPressed));
            GetNode<Button>("Table3D/HUD/PassPlay").Connect("pressed", this, nameof(OnPassPlayPressed));
            LifeChange = GetNode<Label>("Table3D/HUD/LifeChange");
        }

        private void OnPassPlayPressed() => PassPlayPressed();
        private void OnEndTurnPressed() => EndTurnPressed();
    }
}