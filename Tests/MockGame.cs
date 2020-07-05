using CardGame.Client.Room;
using CardGame.Server.Game;
using Godot;
using Messenger = CardGame.Server.Game.Messenger;

namespace CardGame.Tests
{
    public class MockGame: Game
    {
        public Player GetPlayerView() => Player;
        public Player GetOpponentView() => Opponent;

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