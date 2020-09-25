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
            Console.WriteLine($"Setting Up State At {OS.GetTicksUsec() / 1000000}");
            Helper = new HelperNode();
            gfx.InterpolateCallback(Helper, 0.01F, nameof(HelperNode.SetState), Player, State);
        }
        
        private class  HelperNode: Node
        {
            public void SetState(Player player, States state)
            {
                Console.WriteLine($"State set at {OS.GetTicksUsec()  / 1000000}");
                player.PlayerState.State = state;
                //QueueFree();
            }
        }
    }
}