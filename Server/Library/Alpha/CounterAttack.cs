using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;

namespace CardGame.Server
{
    public class CounterAttack: Support
    {
        public CounterAttack(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "CounterAttack";
            SetCode = SetCodes.Alpha_CounterAttack;
            Skill = new DestroyAttacker(this);
        }

        private class DestroyAttacker: Skill
        {
            public DestroyAttacker(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Support);
                Triggers.Add(GameEvents.DeclareAttack);
            }

            protected override void _Resolve()
            {
                Destroy(Match.Attacking);
            }
        }
    }
}