using System.Collections.Generic;
using System.Linq.Expressions;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;

namespace CardGame.Server
{
    public class WrongWay : Support
    {
        public WrongWay()
        {
            Title = "WrongWay";
            SetCode = SetCodes.Alpha_WrongWay;
            AddSkill(new ReturnTarget());
        }

        private class ReturnTarget : Skill
        {
            protected override void _SetUp()
            {
                var targets = new List<Card>();
                foreach (var card in Opponent.Field)
                {
                    targets.Add(card);
                }

                foreach (var card in Controller.Field)
                {
                    targets.Add(card);
                }

                SetTargets(targets);
            }

            protected override void _Resolve()
            {
                Bounce(Target);
            }
        }
    }
}