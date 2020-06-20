using System.Collections.Generic;
using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Commands
{
    public class Move: GameEvent, ICommand
    {
        // The intiiator of the action (either a player or effect)
        public readonly ISource Source;
        
        // The Player ("receiver") performing the action
        public readonly Player Player;

        // The Card the action is being performed on
        public readonly Card Card;
        
        // Where they card was
        public readonly List<Card> Origin;
        
        // Where the card is being moved to
        public readonly List<Card> Destination;

        public Move(ISource source, Player player, Card card, List<Card> destination)
        {
            Source = source;
            Player = player;
            Card = card;
            Origin = card.Zone;
            Destination = destination;
        }

        public void Execute() => Player.Move(Origin, Card, Destination);

        public void Undo() => Player.Move(Destination, Card, Origin);
    }
}