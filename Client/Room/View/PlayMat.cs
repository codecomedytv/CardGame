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

        public override void _Ready()
        {
            Player = GetNode<Player>("Player");
            Opponent = GetNode<Player>("Opponent");
        }
        
    }
}