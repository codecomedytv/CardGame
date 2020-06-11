using Godot.Collections;

namespace CardGame.Server.Game.Commands
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
            Message = new Network.Messages.LoseLife(lifeLost);
        }

        public void Execute() => Player.Health -= LifeLost;
        public void Undo() => Player.Health += LifeLost;
    }
}