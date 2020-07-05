using Godot;

namespace CardGame.Client.Room.Commands
{
    public class LoseLife: Command
    {
        private readonly Player Player;
        private readonly int LifeLost;
        public LoseLife(Player player, int lifeLost)
        {
            Player = player;
            LifeLost = lifeLost;
        }

        protected override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}