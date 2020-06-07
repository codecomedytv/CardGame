﻿using System.Collections.Generic;
using Godot.Collections;

namespace CardGame.Server.Commands
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

        public void Execute()
        {
            Player.Move(PreviousZone, Card, Card.Owner.Deck);
        }

        public void Undo()
        {
            Player.Move(Card.Owner.Deck, Card, PreviousZone);
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.ReturnToDeck;
            message.Player["args"] = new Array{Card.Id};
            message.Opponent["command"] = GameEvents.OpponentReturnedToDeck;
            message.Opponent["args"] = new Array(Card.Serialize());
            return message;
        }
    }
}