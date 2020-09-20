using System;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class Table: Spatial
    {
        [Export] private PackedScene TableScene;
        [Export] private NodePath Player;
        [Export] private NodePath Opponent;
        [Export] private NodePath LifeChangeLabel;
        [Export] private NodePath CardView;
        [Export] private NodePath PassPlay;
        [Export] private NodePath EndTurn;
        public IPlayerView PlayerView { get; }
        public IPlayerView OpponentView { get; }

        public Action PassPlayPressed;
        public Action EndTurnPressed;
        public Label LifeChange;
        public CardViewer CardViewer;
        public TextureRect ActivationView;

        public static Table Create()
        {
            return new Table();
        }
        
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
            CardViewer = GetNode<CardViewer>("Table3D/HUD/CardViewer");
            ActivationView = GetNode<TextureRect>("Table3D/HUD/ActivationView");
        }

        private void OnPassPlayPressed() => PassPlayPressed();
        private void OnEndTurnPressed() => EndTurnPressed();
    }
}