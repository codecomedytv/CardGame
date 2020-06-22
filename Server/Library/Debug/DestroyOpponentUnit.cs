using System.Diagnostics.Tracing;
using System.Linq;
using CardGame.Server.Room;
using CardGame.Server.Room.Cards;
using CardGame.Server.Room.Commands;
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
                var units = Opponent.Field;
                SetTargets(units);
                CanBeUsed = units.Count > 0;
            }

            protected override void _Resolve()
            {
                Controller.DeclarePlay(new Move(Card, Target.Owner, Target, Target.Owner.Graveyard));
            }
        }
    }
}