using System.Threading.Tasks;
using CardGame.Client.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class Draw: Command
    {
        private readonly Player Player;
        private readonly Card Card;

        public Draw(Player player, Card card)
        {
            Player = player;
            Card = card;
        }
        protected override async Task<object[]> Execute()
        {
            Card.Player = Player;
            Player.DeckCount -= 1;
            var originalColor = Card.Modulate;
            Card.Modulate = Colors.Transparent;
            Player.Deck.Add(Card);
            Card.Position = Player.Deck.Position;
            MoveCard(Card, Player.Hand);
            // TODO: We need to add a modulate from the card back to the card front
            QueueProperty(Card, "modulate", Colors.Transparent, originalColor, 0.1F, 0.1F);
            return await Start();

        }
    }
}
