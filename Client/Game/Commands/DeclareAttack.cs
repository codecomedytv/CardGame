using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game
{
    public class DeclareAttack: Command
    {
        private readonly Card Attacker;
        private readonly Card Defender;
        private readonly BattleSystem BattleSystem;

        public DeclareAttack(Card attacker, Card defender, BattleSystem battleSystem)
        {
            Attacker = attacker;
            Defender = defender;
            BattleSystem = battleSystem;
        }
        protected override void SetUp(Effects gfx)
        {
            gfx.InterpolateCallback(BattleSystem, 0.1F, nameof(BattleSystem.OnAttackerSelected), Attacker);
            gfx.InterpolateCallback(BattleSystem, 0.1F, nameof(BattleSystem.OnDefenderSelected), Defender);
        }
    }
}

