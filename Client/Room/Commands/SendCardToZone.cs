using System.Threading.Tasks;
using CardGame.Client.Cards;
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
            MoveCard(Card, zone);
            QueueCallback(zone, 0.1F, nameof(Zone.Sort));
            Gfx.Start();
            return await ToSignal(Gfx, "tween_all_completed");
        }
    }
}
