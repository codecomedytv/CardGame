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
            QueueProperty(Card, "rect_global_position", Card.RectGlobalPosition, FuturePosition(Player.Units), 0.2F, 0);
            
            // This really doesn't need to be a callback does it?
            QueueCallback(Player.Hand, 0.2F, nameof(Zone.Remove), Card);
            QueueCallback(Player.Units, 0.2F, nameof(Zone.Add), Card);
            Gfx.Start();
            return await ToSignal(Gfx, "tween_all_completed");
        }
    }
}