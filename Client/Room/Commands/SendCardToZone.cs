using System.Threading.Tasks;
using CardGame.Client.Library.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class SendCardToZone: Command
    {
        private readonly Player Player;
        private readonly Card Card;
        private readonly ZoneIds ZoneId;
        public SendCardToZone(Player player, Card card, ZoneIds zoneId)
        {
            Player = player;
            Card = card;
            ZoneId = zoneId;
        }

        protected override async Task<object[]> Execute()
        {
            var zone = GetZone(Player, ZoneId);
            QueueCallback(Card, 0, nameof(Card.MoveZone), Card.GetParent(), zone);
            QueueProperty(Card, "RectGlobalPosition", Card.RectGlobalPosition, zone.Position, 0.1F, 0);
            Gfx.Start();
            return await ToSignal(Gfx, "tween_all_completed");
        }
    }
}
