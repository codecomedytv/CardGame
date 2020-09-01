using System;
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
            throw new NotImplementedException("Method Requires This To Refactor Players To Avoid Leaky Abstraction");
            /*
            var newLife = GD.Str(LifeCount.Text.ToInt() - lifeLost);
            var percentage = 100 - (int) ((lifeLost / 8000F) * 100);
            LifeChange.Text = $"- {lifeLost}";
            gfx.InterpolateCallback(LifeChange, 0.1F, "set_visible", true);
            gfx.InterpolateCallback(LifeCount, 0.3F, "set_text", newLife);
				
            gfx.InterpolateProperty(LifeBar, "value", (int) LifeBar.Value, percentage, 0.3F);
            gfx.InterpolateCallback(LifeChange, 0.5F, "set_visible", false);
            */
        }
    }
}
