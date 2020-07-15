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
            var mod = IsOpponent ? 150 : -150;
            var attackerDestination = new Vector2(Attacker.RectPosition.x, Attacker.RectPosition.y + mod);
            QueueProperty(Attacker, "rect_position", attackerLocation, attackerDestination, 0.1F, 0.1F);
            QueueProperty(Attacker, "rect_position", attackerDestination, attackerLocation, 0.1F, 0.2F);
            QueueCallback(Attacker, 0.2F, nameof(Card.Deselect));
            QueueCallback(Defender, 0.2F, nameof(Card.Deselect));
            QueueCallback(Attacker, 0.2F, nameof(Card.StopAttacking));
            QueueCallback(Defender, 0.2F, nameof(Card.StopDefending));
            return await Start();

        }
    }
}