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
            Message = new Network.Messages.SetSupport(card);
        }

        public void Execute()
        {
            Player.Move(PreviousZone, Card, Player.Support);
        }

        public void Undo()
        {
           Player.Move(Player.Support, Card, PreviousZone);
        }
        
        
    }
}