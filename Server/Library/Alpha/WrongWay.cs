﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;

namespace CardGame.Server
{
    public class WrongWay : Support
    {
        public WrongWay(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "WrongWay";
            SetCode = SetCodes.AlphaWrongWay;
            Skill = new ReturnTarget(this);
        }

        private class ReturnTarget : Manual
        {
            public ReturnTarget(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Support);
            }
            
            protected override bool _SetUp()
            {
                AddTargets(Opponent.Field);
                AddTargets(Controller.Field);
                return ValidTargets.Count > 0;
            }

            protected override void _Resolve()
            {
                Bounce(Target);
            }
        }
    }
}