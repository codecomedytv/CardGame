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

        protected override async Task<object[]> Execute()
        {
            Player.Damage.Text = $"-{LifeLost}";
            QueueCallback(this, 0, nameof(ShowDamage), true);
            QueueCallback(this, 0.3F, nameof(ShowDamage), false);
            QueueCallback(Player, 0, nameof(Player.LoseLife), LifeLost);
            return await Start();
        }
        
        private void ShowDamage(bool show)
        {
            Player.Damage.Visible = show;
        }
    }
}
