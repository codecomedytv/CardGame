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
            //Player.PlayerState.State = State;
            Helper = new HelperNode();
            gfx.InterpolateCallback(Helper, 0.3F, nameof(HelperNode.SetState), Player, State);
            Console.WriteLine($"{gfx.GetRunTime()} is runtime of set state");
        }
        
        private class  HelperNode: Node
        {
            public void SetState(Player player, States state)
            {
                player.PlayerState.State = state;
                Console.WriteLine($"State set at {OS.GetTicksUsec()}");
                QueueFree();
            }
        }
    }
}