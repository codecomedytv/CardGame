using System;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class SetState: Command
    {
        private readonly Player Player;
        private readonly States State;
        private HelperNode Helper;

        public SetState(Player player, States state)
        {
            Player = player;
            State = state;
        }

        protected override void SetUp(Effects gfx)
        {
            Helper = new HelperNode();
            gfx.InterpolateCallback(Helper, 0.01F, nameof(HelperNode.SetState), Player, State);
        }
        
        private class  HelperNode: Node
        {
            public void SetState(Player player, States state)
            {
                player.PlayerState.State = state;
            }
        }
    }
}