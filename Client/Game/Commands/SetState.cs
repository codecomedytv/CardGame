using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class SetState: Command
    {
        private readonly Player Player;
        private readonly States State;

        public SetState(Player player, States state)
        {
            Player = player;
            State = state;
        }

        protected override void SetUp(Effects gfx)
        {
            Player.PlayerState.State = State;
        }
    }
}