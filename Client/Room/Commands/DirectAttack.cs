using System.Threading.Tasks;
using CardGame.Client.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class DirectAttack: Command
    {
        public readonly bool IsOpponent;
        public readonly Card Attacker;

        public DirectAttack(bool isOpponent, Card attacker)
        {
            IsOpponent = isOpponent;
            Attacker = attacker;
        }
        
        protected override async Task<object[]> Execute()
        {
            var direction =  (float) (IsOpponent ? 0.0 : Sfx.GetTree().Root.GetVisibleRect().Size.y);
            var destination = new Vector2(Attacker.Position.x, direction);
            QueueCallback(Attacker, 0F, nameof(Card.Deselect));
            QueueCallback(Attacker.Player.Opponent, 0F, nameof(Player.StopShowingAsTargeted));
            QueueProperty(Attacker, "rect_position", Attacker.Position, destination, 0.1F, 0.1F);
            QueueProperty(Attacker, "rect_position", destination, Attacker.Position, 0.1F, 0.2F);
            return await Start();
        }
    }
}
