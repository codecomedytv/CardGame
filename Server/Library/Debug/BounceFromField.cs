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
            AddSkill(new BounceSkill());
        }

        private class BounceSkill : Skill
        {
            protected override void _SetUp()
            {
                SetTargets(Opponent.Field.ToList());
                CanBeUsed = Opponent.Field.Count > 0;
            }

            protected override void _Resolve()
            {
                Bounce(Target);
            }
        }
    }
}