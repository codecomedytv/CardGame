using CardGame.Client.Players;
using CardGame.Client.Room;
using CardGame.Server.Game;
using Godot;

namespace CardGame.Tests
{
    public class MockGame: Game
    {
        public View GetPlayerView() => Player.View;
        public View GetOpponentView() => Opponent.View;

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