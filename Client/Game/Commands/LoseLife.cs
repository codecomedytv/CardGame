using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class LoseLife: Command
    {
        private IPlayer Player;
        private int LifeLost;
        
        public LoseLife(IPlayer player, int lifeLost)
        {
            Player = player;
            LifeLost = lifeLost;
        }
        protected override void SetUp(Tween gfx)
        {
            var finalValue = Player.Health - LifeLost;
            gfx.InterpolateProperty((Object) Player, nameof(IPlayer.Health), Player.Health, finalValue, 1);
        }
    }
}
