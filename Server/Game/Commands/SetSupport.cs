using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.Game.Commands
{
    public class SetSupport : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;
        public readonly List<Card> PreviousZone;

        public SetSupport(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
            PreviousZone = card.Zone;
        }

        public void Execute()
        {
            Player.Move(PreviousZone, Card, Player.Support);
        }

        public void Undo()
        {
           Player.Move(Player.Support, Card, PreviousZone);
        }
        
        // public override Message GetMessage()
        // {
        //     var message = new Message();
        //     message.Player["command"] = GameEvents.SetFaceDown;
        //     message.Player["args"] = new Array{Card.Id};
        //     message.Opponent["command"] = GameEvents.OpponentSetFaceDown;
        //     message.Opponent["args"] = new Array();
        //     return message;
        // }
    }
}