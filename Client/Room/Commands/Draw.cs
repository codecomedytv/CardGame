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
            Player.Hand.AddChild(Card);
            Sort(Player.Hand);
            var destination = Card.RectGlobalPosition;
            Card.RectGlobalPosition = Player.Deck.RectGlobalPosition;
            QueueCallback(Player.Deck, 0, "set_text", Player.DeckCount.ToString());
            QueueProperty(Card, "RectGlobalPosition", Player.Deck.RectGlobalPosition, destination, 0.2F, 0.2F);
            QueueProperty(Card, nameof(Control.Modulate), Colors.Transparent, originalColor, 0.1F, 0.2F);
            QueueCallback(this, 0.4F,"Sort", Player.Hand);
            Gfx.Start();
            return await ToSignal(Gfx, "tween_all_completed");
        }
    }
}