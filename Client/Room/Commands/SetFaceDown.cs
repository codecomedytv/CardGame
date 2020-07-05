using System.Threading.Tasks;
using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class SetFaceDown: Command
    {
        private readonly Player Player;
        private readonly Card Card;
        private readonly bool IsOpponent;
        public SetFaceDown(Player player, Card card, bool isOpponent)
        {
            Player = player;
            Card = card;
            IsOpponent = isOpponent;
        }

        protected override Task<object[]> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}