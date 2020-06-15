using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.Game.Commands
{
    public class  Discard : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;

        public Discard(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
        }

        public void Execute() => Player.Move(Player.Hand, Card, Player.Graveyard);
        public void Undo() => Player.Move(Player.Graveyard, Card, Player.Hand);
    }
}