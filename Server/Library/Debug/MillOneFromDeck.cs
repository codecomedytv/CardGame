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
            Skill = new MillCard(this);
        }

        private class  MillCard: Skill
        {
            public MillCard(Card card)
            {
                Card = card;
            }
            protected override void _Resolve()
            {
                Mill(Controller.Deck.Top);
            }
        }
    }
}