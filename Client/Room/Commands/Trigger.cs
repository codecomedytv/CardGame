using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class Trigger: Command
    {
        private readonly Card Card;
        private readonly int PositionInLink;
        public Trigger(Card card, int positionInLink)
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