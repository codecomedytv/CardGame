using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;

namespace CardGame.Server
{
    public class BounceFromField : Support
    {
        public BounceFromField()
        {
            Title = "Debug.BounceFromField";
            SetCode = SetCodes.DebugBounceFromField;
            AddSkill(new BounceSkill());
        }

        private class BounceSkill : Skill
        {

            public override void _SetUp()
            {
                SetTargets(Opponent.Field.ToList());
                CanBeUsed = Opponent.Field.Count > 0;
            }

            protected override void _Resolve()
            {
                Controller.DeclarePlay(new Bounce(Card, Controller, Target));
            }
        }
    }
}