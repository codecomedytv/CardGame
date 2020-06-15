using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.Game.Commands
{

    public class Mill: GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;

        public Mill(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
        }
        
        public void Execute() => Player.Move(Card.Owner.Deck, Card, Card.Owner.Graveyard);
        public void Undo() => Player.Move(Card.Owner.Graveyard, Card, Card.Owner.Deck);
        
    }
}