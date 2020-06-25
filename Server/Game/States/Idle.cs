using System.Collections.Generic;
using CardGame.Server.Game;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using Godot;

namespace CardGame.Server.States
{
    public class Idle: State
    {
        public override void OnEnter(Player player)
        {
        }
        
        public override string ToString()
        {
            return "Idle";
        }
    }
}