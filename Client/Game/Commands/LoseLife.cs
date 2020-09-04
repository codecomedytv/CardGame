﻿using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class LoseLife: Command
    {
        private readonly BasePlayer Player;
        private readonly int LifeLost;
        private readonly Table Table;
        
        public LoseLife(BasePlayer player, int lifeLost, Table table)
        {
            Player = player;
            LifeLost = lifeLost;
            Table = table;
        }
        protected override void SetUp(Tween gfx)
        {
            var finalValue = Player.Health - LifeLost;
            Table.LifeChange.Text = $"-{LifeLost}";
            var originalPosition = Table.LifeChange.RectPosition;
            var yMod = Player is Player ? 200 : -200;
            Table.LifeChange.RectPosition += new Vector2(0, yMod);
            gfx.InterpolateCallback(Table.LifeChange, 0, "set_visible", true);
            gfx.InterpolateProperty(Player, nameof(BasePlayer.Health), Player.Health, finalValue, 1);
            gfx.InterpolateCallback(Table.LifeChange, 1, "set_visible", false);
            gfx.InterpolateProperty(Table.LifeChange, "rect_position", Table.LifeChange.RectPosition, originalPosition,
                0.1F, Tween.TransitionType.Back, Tween.EaseType.In, 1.1F);
        }
    }
}
