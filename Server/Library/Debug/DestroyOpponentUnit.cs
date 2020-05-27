using System.Diagnostics.Tracing;
using System.Linq;

namespace CardGame.Server
{
    public class DestroyOpponentUnit : Support
    {
        public DestroyOpponentUnit()
        {
            Title = "Debug.DestroyOpponentUnit";
            SetCode = SetCodes.DebugDestroyOpponentUnit;
            AddSkill(new DestroyUnit());
        }

        public class DestroyUnit : Skill
        {
            public override void _SetUp()
            {
                var units = Opponent.Field.Where(u => !u.HasTag(Tag.CannotBeTargeted)).ToList();
                SetTargets(units);
                _SetLegal(units.Count > 0);
            }

            public override void _Resolve()
            {
                Controller.DestroyUnit((Unit)GameState.Target);
            }
        }
    }
}