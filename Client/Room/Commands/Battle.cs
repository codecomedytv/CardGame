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
            var attackerDestination = new Vector2(Attacker.RectPosition.x, Attacker.RectPosition.y - 150);
            var defenderDestination = new Vector2(Defender.RectPosition.x, Defender.RectPosition.y + 50);
            if (IsOpponent)
            {
                // We were going the wrong way, so we reverse and compensate
                attackerDestination.y += 150;
              //  defenderDestination.y -= 100;
            }
            QueueProperty(Attacker, "rect_position", attackerLocation, attackerDestination, 0.1F, 0.1F);
            //QueueProperty(Defender, "rect_position", defenderLocation, defenderDestination, 0.1F, 0.1F);
            QueueProperty(Attacker, "rect_position", attackerDestination, attackerLocation, 0.1F, 0.2F);
            //QueueProperty(Defender, "rect_position", defenderDestination, defenderLocation, 0.1F, 0.2F);
            QueueCallback(Attacker, 0.2F, nameof(Card.Deselect));
            QueueCallback(Defender, 0.2F, nameof(Card.Deselect));
            QueueCallback(Attacker, 0.2F, nameof(Card.StopAttacking));
            QueueCallback(Defender, 0.2F, nameof(Card.StopDefending));
            return await Start();

        }
    }
}