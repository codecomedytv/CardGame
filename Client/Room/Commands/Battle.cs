using System.Threading.Tasks;
using CardGame.Client.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class Battle: Command
    {
        private readonly Card Attacker;
        private readonly Card Defender;
        private readonly bool IsOpponent;

        public Battle(Card attacker, Card defender, bool isOpponent)
        {
            Attacker = attacker;
            Defender = defender;
            IsOpponent = isOpponent;
        }
        protected override async Task<object[]> Execute()
        {
            var attackerLocation = Attacker.RectPosition;
            var defenderLocation = Defender.RectPosition;
            var attackerDestination = new Vector2(Attacker.RectPosition.x, Attacker.RectPosition.y - 50);
            var defenderDestination = new Vector2(Defender.RectPosition.x, Defender.RectPosition.y + 50);
            if (IsOpponent)
            {
                // We were going the wrong way, so we reverse and compensate
                attackerDestination.y += 100;
                defenderDestination.y -= 100;
            }
            QueueProperty(Attacker, "RectPosition", attackerLocation, attackerDestination, 0.1F, 0.1F);
            QueueProperty(Defender, "RectPosition", defenderLocation, defenderDestination, 0.1F, 0.1F);
            QueueProperty(Attacker, "RectPosition", attackerDestination, attackerLocation, 0.1F, 0.2F);
            QueueProperty(Defender, "RectPosition", defenderDestination, defenderLocation, 0.1F, 0.2F);

            Gfx.Start();
            return await ToSignal(Gfx, "tween_all_completed");
        }
    }
}