using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class Deploy: Command
    {
        private readonly Player Player;
        private readonly Card Card;

        public Deploy(Player player, Card card)
        {
            Player = player;
            Card = card;
        }
        public override void Execute(Tween gfx)
        {
            throw new System.NotImplementedException();
        }
    }
}