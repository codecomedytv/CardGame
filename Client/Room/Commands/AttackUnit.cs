using System.Threading.Tasks;
using CardGame.Client.Cards;

namespace CardGame.Client.Room.Commands
{
    public class AttackUnit: Command
    {
        public readonly Card Attacker;
        public readonly Card Defender;
        
        public AttackUnit(Card attacker, Card defender)
        {
            Attacker = attacker;
            Defender = defender;
        }
        protected override async Task<object[]> Execute()
        {
            QueueCallback(Attacker, 0.0F, nameof(Card.AttackUnit), Defender);
            return await Start();
        }
    }
}
