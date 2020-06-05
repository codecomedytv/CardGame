using System.Diagnostics.Tracing;
using System.Linq;
using Godot;

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

        private class DestroyUnit : Skill
        {
            public override void _SetUp()
            {
                var units = Opponent.Field.Where(u => !u.HasTag(Tag.CannotBeTargeted)).ToList();
                SetTargets(units);
                CanBeUsed = units.Count > 0;
            }

            protected override void _Resolve()
            {
                Controller.DestroyUnit((Unit)GameState.Target);
            }
        }
    }
}