using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game
{
    public class Battle: Command
    {
        private readonly Card Attacker;
        private readonly Card Defender;
        
        public Battle(Card attacker, Card defender)
        {
            Attacker = attacker;
            Defender = defender;
        }
        protected override void SetUp(Effects gfx)
        {
            var attackerY = Attacker.Controller.IsUser? 0.5F: 1.75F;
            var defenderY = Attacker.Controller.IsUser? 0.5F: 1.75F;
            var attackerDestination = new Vector3(2.5F, attackerY, Attacker.Translation.z);
            var defenderDestination = new Vector3(2.5F, defenderY, Defender.Translation.z);

            gfx.InterpolateProperty(Attacker, "translation", Attacker.Translation, attackerDestination, 0.1F);
            gfx.InterpolateProperty(Defender, "translation", Defender.Translation, defenderDestination, 0.1F);
            // Insert Sound
            gfx.InterpolateProperty(Attacker, "translation", attackerDestination, Attacker.Translation, 0.1F, 
                Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);
            gfx.InterpolateProperty(Defender, "translation", defenderDestination, Defender.Translation, 0.1F,
                Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);
            
            gfx.InterpolateCallback(Attacker, 0.4F, nameof(Card.StopAttack));
            gfx.InterpolateCallback(Defender, 0.4F, nameof(Card.StopDefend));
        }
    }
}