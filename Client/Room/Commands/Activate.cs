using System.Threading.Tasks;
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
        protected override Task<object[]> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}