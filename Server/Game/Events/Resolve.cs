﻿#nullable enable
using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Events
{
    public class ResolveCard: Event
    {
        public readonly ISource Source;
        public readonly Card Card;
        public readonly Card? Target;

        public ResolveCard(Card card, Card? target)
        {
            Source = card;
            Card = card;
            Target = target;
        }

        private int TargetId => Target?.Id ?? 0;
        
    }
}