using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game
{
    public class DeclareAttack: Command
    {
        private readonly Card Attacker;
        private readonly Card Defender;

        public DeclareAttack(Card attacker, Card defender)
        {
            Attacker = attacker;
            Defender = defender;
        }
        protected override void SetUp(Effects gfx)
        {
            gfx.InterpolateCallback(Attacker, 0.1F, nameof(Card.Attack));
            gfx.InterpolateCallback(Defender, 0.1F, nameof(Card.Defend));
        }
    }
}

