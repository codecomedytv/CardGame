using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.Game.Commands
{
    public class ReturnToDeck : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly List<Card> PreviousZone;
        public readonly Card Card;

        public ReturnToDeck(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
            PreviousZone = card.Zone;
        }

        public void Execute() => Player.Move(PreviousZone, Card, Card.Owner.Deck);
        public void Undo() => Player.Move(Card.Owner.Deck, Card, PreviousZone);
    }
}