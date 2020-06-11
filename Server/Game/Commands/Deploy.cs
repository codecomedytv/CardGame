using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.Game.Commands
{
    public class Deploy : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;
        public readonly List<Card> PreviousZone;

        public Deploy(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
            PreviousZone = Card.Zone;
        }

        public void Execute()
        {
            Player.Move(PreviousZone, Card, Player.Field);
        }

        public void Undo()
        {
            Player.Move(Player.Field, Card, PreviousZone);
        }

        // public override Message GetMessage()
        // {
        //     var message = new Message();
        //     message.Player["command"] = GameEvents.Deploy;
        //     message.Player["args"] = new Array {Card.Id};
        //     message.Opponent["command"] = GameEvents.OpponentDeploy;
        //     message.Opponent["args"] = new Array {Card.Serialize()};
        //     return message;
        // }
    }
}