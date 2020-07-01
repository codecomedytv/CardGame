﻿using CardGame.Client.Players;
using CardGame.Client.Room;
using Godot;

namespace CardGame.Tests
{
    public class MockGame: Game
    {
        public View GetPlayerView() => Player.View;
        public View GetOpponentView() => Opponent.View;

        public void EndTurn()
        {
            //EmitSignal(nameof(EndedTurn));
        }
    }
}