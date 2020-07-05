using System.Threading.Tasks;
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
        
        protected override Task<object[]> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}