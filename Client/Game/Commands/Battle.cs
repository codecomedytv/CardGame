using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class Battle: xCommand
    {
        private readonly Card Attacker;
        private readonly Card Defender;
        
        public Battle(Card attacker, Card defender)
        {
            Attacker = attacker;
            Defender = defender;
        }
        protected override void SetUp(Tween gfx)
        {
            var attackerY = Attacker.Controller is Opponent ? 1.75F : 0.5F;
            var defenderY = Attacker.Controller is Opponent ? 1.75F : 0.5F;
            var attackerDestination = new Vector3(2.5F, attackerY, Attacker.Translation.z);
            var defenderDestination = new Vector3(2.5F, defenderY, Defender.Translation.z);

            gfx.InterpolateProperty(Attacker, nameof(Translation), Attacker.Translation, attackerDestination, 0.1F);
            gfx.InterpolateProperty(Defender, nameof(Translation), Defender.Translation, defenderDestination, 0.1F);
            // Insert Sound
            gfx.InterpolateProperty(Attacker, nameof(Translation), attackerDestination, Attacker.Translation, 0.1F, 
                Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);
            gfx.InterpolateProperty(Defender, nameof(Translation), defenderDestination, Defender.Translation, 0.1F,
                Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);

            gfx.InterpolateCallback(Attacker, 0.4F, nameof(Card.StopAttacking));
            gfx.InterpolateCallback(Defender, 0.4F, nameof(Card.StopDefending));
        }
    }
}