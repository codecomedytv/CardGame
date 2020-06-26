﻿using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;

namespace CardGame.Server
{
    public class MillOneFromDeck : Support
    {
        public MillOneFromDeck(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.MillOneFromCard";
            SetCode = SetCodes.MillOneFromDeck;
            AddSkill(new MillCard());
        }

        private class  MillCard: Skill
        {
            protected override void _Resolve()
            {
                Mill(Controller.Deck.Top);
            }
        }
    }
}