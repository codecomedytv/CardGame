using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;

namespace CardGame.Server
{
    public class BounceFromField : Support
    {
        public BounceFromField(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.BounceFromField";
            SetCode = SetCodes.DebugBounceFromField;
            Skill = new BounceSkill(this);
        }

        private class BounceSkill : Manual
        {
            public BounceSkill(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Support);
            }
            protected override void _SetUp()
            {
                AddTargets(Opponent.Field);
                CanBeUsed = ValidTargets.Count > 0;
            }

            protected override void _Resolve()
            {
                Bounce(Target);
            }
        }
    }
}