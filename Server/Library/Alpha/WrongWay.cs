﻿using System.Collections.Generic;
using System.Linq.Expressions;

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

        public class ReturnTarget : Skill
        {
            public override void _SetUp()
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

            public override void _Resolve()
            {
                Controller.Bounce(GameState.Target);
            }
        }
    }
}