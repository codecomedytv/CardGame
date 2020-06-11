using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.Game.Commands
{
    public class Bounce : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;
        public readonly List<Card> PreviousZone;
        

        public Bounce(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
            PreviousZone = card.Zone;
        }

        // public override Message GetMessage()
        // {
        //     var message = new Message();
        //     message.Player["command"] = GameEvents.Bounce;
        //     message.Player["args"] = new Array {Card.Id};
        //     message.Opponent["command"] = GameEvents.OpponentBounce;
        //     message.Opponent["args"] = new Array {Card.Id};
        //     return message;
        // }

        public void Execute()
        {
            Player.Move(PreviousZone, Card, Card.Owner.Hand);
        }

        public void Undo()
        {
            Player.Move(Card.Owner.Hand, Card, PreviousZone);
        }
    }
}