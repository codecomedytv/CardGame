using CardGame.Client.Room;
using CardGame.Client.Room.View;
using CardGame.Server.Game;
using Godot;
using Messenger = CardGame.Server.Game.Messenger;
using Player = CardGame.Client.Room.Player;

namespace CardGame.Tests
{
    public class MockGame: Game
    {
        public Player GetPlayerView() => Player;
        public Player GetOpponentView() => Opponent;
        
        public MockGame(PlayMat playMat) : base(playMat)
        {
        }

        public void End()
        {
            EndTurn.EmitSignal("pressed");
        }

        public void Pass()
        {
            OnActionButtonPressed();
        }

        
    }
}