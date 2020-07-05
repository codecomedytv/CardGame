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

        public override void Execute(Tween gfx)
        {
            throw new System.NotImplementedException();
        }
    }
}