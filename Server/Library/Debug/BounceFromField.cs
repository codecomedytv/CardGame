using System.Linq;

namespace CardGame.Server
{
    public class BounceFromField : Support
    {
        public BounceFromField()
        {
            Title = "Debug.BounceFromField";
            SetCode = SetCodes.DebugBounceFromField;
        }

        [Skill]
        public class BounceSkill : Skill
        {
            public override void _SetUp()
            {
                SetTargets(Opponent.Field.ToList());
                CanBeUsed = Opponent.Field.Count > 0;
            }

            public override void _Resolve()
            {
                Controller.Bounce(GameState.Target);
            }
        }
    }
}