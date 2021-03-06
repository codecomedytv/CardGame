﻿using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;

namespace CardGame.Server
{
    public class ReturnCardToDeck: Support
    {
        public ReturnCardToDeck(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.ReturnToDeck";
            SetCode = SetCodes.DebugReturnToDeck;
            Skill = new ReturnCard(this);
        }

        private class ReturnCard : Manual
        {
            public ReturnCard(Card card)
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
                TopDeck(Target);
            }
        }
        
    }
}