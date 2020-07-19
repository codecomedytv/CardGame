using System.Threading.Tasks;
using CardGame.Client.Cards;

namespace CardGame.Client.Room.Commands
{
    public class DirectAttackDeclared: Command
    {
        public readonly Player Player;
        public readonly Card Attacker;

        public DirectAttackDeclared(Player player, Card attacker)
        {
            Player = player;
            Attacker = attacker;
        }
        protected override async Task<object[]> Execute()
        {
            QueueCallback(Attacker, 0.0F, nameof(Card.Select));
            QueueCallback(Player, 0.0F, nameof(Player.ShowAsTargeted));
            return await Start();
        }
    }
}