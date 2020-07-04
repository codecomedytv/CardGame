using CardGame.Client.Room;
using CardGame.Server.Game;
using Godot;

namespace CardGame.Tests
{
    public class MockGame: Game
    {
        public Player GetPlayerView() => Player;
        public Player GetOpponentView() => Opponent;

        public void End()
        {
            OnEndTurn();
        }

        public void Pass()
        {
            OnActionButtonPressed();
        }
    }
}