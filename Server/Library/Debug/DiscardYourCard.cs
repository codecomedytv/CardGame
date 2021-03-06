﻿using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;

namespace CardGame.Server
{
    public class DiscardYourCard : Support
    {
        public DiscardYourCard(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.DiscardYourCard";
            SetCode = SetCodes.DebugDiscardYourCard;
            Skill = new DiscardCard(this);
        }

        private class DiscardCard : Manual
        {
            public DiscardCard(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Support);

            }
            protected override bool _SetUp()
            {
                AddTargets(Controller.Hand);
                return ValidTargets.Count > 0;
            }

            protected override void _Resolve()
            {
                Discard(Target);
            }
        }
    }
}