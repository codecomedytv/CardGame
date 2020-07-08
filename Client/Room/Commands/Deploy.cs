using System.Threading.Tasks;
using CardGame.Client.Cards;
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
        protected override async Task<object[]> Execute()
        {
            MoveCard(Card, Player.Units);
            return await Start();

        }
    }
}