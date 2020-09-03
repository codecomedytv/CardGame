using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class DeclareDirectAttack: Command
    {
        private readonly Player Player;
        private readonly Card Attacker;
        private readonly BattleSystem BattleSystem;

        public DeclareDirectAttack(Player player, Card attacker, BattleSystem battleSystem)
        {
            Player = player;
            Attacker = attacker;
            BattleSystem = battleSystem;
        }
        protected override void SetUp(Tween gfx)
        {
            gfx.InterpolateCallback(BattleSystem, 0.1F, nameof(BattleSystem.OnAttackerSelected), Attacker);
            gfx.InterpolateCallback(Player, 0.1F, nameof(Player.Defend));
        }
    }
}