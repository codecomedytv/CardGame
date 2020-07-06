using System.Threading.Tasks;
using CardGame.Client.Library.Cards;
using CardGame.Tests.Scripts;
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
            Player.DeckCount -= 1;
            var originalColor = Card.Modulate;
            Card.Modulate = Colors.Transparent;
            Player.Hand.Add(Card);
            Player.Hand.Sort();
            var destination = Card.RectGlobalPosition;
            Card.RectGlobalPosition = Player.Deck.Position;
            QueueProperty(Card, "RectGlobalPosition", Player.Deck.Position, destination, 0.1F, 0.1F);
            QueueProperty(Card, nameof(Control.Modulate), Colors.Transparent, originalColor, 0.1F, 0.1F);
            Gfx.Start();
            return await ToSignal(Gfx, "tween_all_completed");
        }
    }
}