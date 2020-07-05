using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class Activate: Command
    {
        private readonly Card Card;
        private readonly int PositionInLink;

        public Activate(Card card, int positionInLink)
        {
            Card = card;
            PositionInLink = positionInLink;
        }
        public override void Execute(Tween gfx)
        {
            throw new System.NotImplementedException();
        }
    }
}