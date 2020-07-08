using System.Linq;
using System.Threading.Tasks;
using CardGame.Client.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class SetFaceDown: Command
    {
        private readonly Player Player;
        private readonly Card Card;
        private readonly bool IsOpponent;
        public SetFaceDown(Player player, Card card, bool isOpponent)
        {
            Player = player;
            Card = card;
            IsOpponent = isOpponent;
        }

        protected override async Task<object[]> Execute()
        {
            if (IsOpponent)
            {
                var toBeReplaced = Player.Hand.Last();
                Player.Hand.Remove(toBeReplaced);
                toBeReplaced.Free(); // This may cause problems if we implement undo
                Player.Hand.Add(Card);
                Player.Hand.Sort();
            }
            QueueCallback(Card, 0, nameof(Card.FlipFaceDown));
            MoveCard(Card, Player.Support);
            Gfx.Start();
            return await ToSignal(Gfx, "tween_all_completed");
        }
    }
}