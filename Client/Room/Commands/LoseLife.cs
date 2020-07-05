using System.Threading.Tasks;
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

        protected override Task<object[]> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}