using System.Diagnostics.Tracing;
using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;
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
            protected override void _SetUp()
            {
                var units = Opponent.Field;
                SetTargets(units.ToList());
                CanBeUsed = units.Count > 0;
            }

            protected override void _Resolve()
            {
                Destroy(Target);
            }
        }
    }
}