using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class LoseLife: Command
    {
        private BasePlayer Player;
        private int LifeLost;
        
        public LoseLife(BasePlayer player, int lifeLost)
        {
            Player = player;
            LifeLost = lifeLost;
        }
        protected override void SetUp(Tween gfx)
        {
            var finalValue = Player.Health - LifeLost;
            gfx.InterpolateProperty((Object) Player, nameof(BasePlayer.Health), Player.Health, finalValue, 1);
        }
    }
}
