using System.Threading.Tasks;
using CardGame.Client.Library.Cards;
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
                var toBeReplaced = Player.Hand.GetChild(Player.Hand.GetChildCount() - 1);
                Player.Hand.RemoveChild(toBeReplaced);
                toBeReplaced.Free(); // This may cause problems if we implement undo
                Player.Hand.AddChild(Card);
                Sort(Player.Hand);
            }
            QueueProperty(Card, "RectGlobalPosition", Card.RectGlobalPosition, FuturePosition(Player.Support), 0.2F, 0);
            QueueCallback(Card, 0, nameof(Card.FlipFaceDown));
            QueueCallback(Card.GetParent(), 0.2F, "remove_child", Card);
            QueueCallback(Player.Support, 0.2F, "add_child", Card);
            Gfx.Start();
            return await ToSignal(Gfx, "tween_all_completed");
        }
    }
}