using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class DirectAttack: Command
    {
        private readonly BasePlayer Player;
        private readonly Card Attacker;
        private readonly BattleSystem BattleSystem;

        public DirectAttack(BasePlayer player, Card attacker, BattleSystem battleSystem)
        {
            Player = player;
            Attacker = attacker;
            BattleSystem = battleSystem;
        }
        protected override void SetUp(Tween gfx)
        {
            var destination = Attacker.Controller is Opponent? new Vector3(2.5F, -2.95F, 1) :  new Vector3(2.5F, 9F, Attacker.Translation.z);

            gfx.InterpolateProperty(Attacker, nameof(Translation), Attacker.Translation, destination, 0.1F);
            gfx.InterpolateProperty(Attacker, nameof(Translation), destination, Attacker.Translation, 0.1F,
                Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);
            gfx.InterpolateCallback(BattleSystem, 0.2F, nameof(BattleSystem.OnAttackStopped), Attacker, null);
            gfx.InterpolateCallback(BattleSystem, 0.2F, nameof(BattleSystem.OnDirectAttackStopped), Player);
        }
    }
}