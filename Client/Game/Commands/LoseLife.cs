using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class LoseLife: xCommand
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
            Player.LoseLife(LifeLost, gfx);
        }
    }
}
