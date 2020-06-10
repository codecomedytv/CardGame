using System.Collections;
using Godot.Collections;

namespace CardGame.Server.Commands
{
    public class LoseLife : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly int LifeLost;

        public LoseLife(ISource source, Player player, int lifeLost)
        {
            Source = source;
            Player = player;
            LifeLost = lifeLost;
        }

        public void Execute()
        {
            Player.Health -= LifeLost;
        }

        public void Undo()
        {
            Player.Health += LifeLost;
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.LoseLife;
            message.Player["args"] = new Array{LifeLost};
            message.Opponent["command"] = GameEvents.NoOp;
            message.Opponent["args"] = new Array();
            return message;
        }
    }
}