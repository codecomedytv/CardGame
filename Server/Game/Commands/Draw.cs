using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.Game.Commands
{
    public class Draw : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;

        public Draw(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
            Message = new Network.Messages.Draw(card);
        }

        public void Execute() => Player.Move(Player.Deck, Card, Player.Hand);
        public void Undo() => Player.Move(Player.Hand, Card, Player.Deck);
        
    }
}