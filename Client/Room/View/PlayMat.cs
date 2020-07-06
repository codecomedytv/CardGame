using System;
using Godot;
using Godot.Collections;

namespace CardGame.Client.Room.View
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PlayMat: Control
    {
        public Player Player;
        public Player Opponent;
        public CardViewer CardViewer;
        public Button PassPriority;
        public AnimatedSprite ActionButtonAnimation;
        public Button EndTurn;
        public Label DisqualificationNotice;

        public override void _Ready()
        {
            Player = GetNode<Player>("Player");
            Opponent = GetNode<Player>("Opponent");
            CardViewer = GetNode<CardViewer>("Background/CardViewer");
            PassPriority = GetNode<Button>("Background/ActionButton");
            ActionButtonAnimation = PassPriority.GetNode<AnimatedSprite>("Glow");
            EndTurn = GetNode<Button>("Background/EndTurn");
            DisqualificationNotice = GetNode<Label>("Disqualified");
        }
        
    }
}