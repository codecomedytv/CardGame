using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game
{
    public class OpponentSetFaceDown: Command
    {
        private readonly Opponent Opponent;

        public OpponentSetFaceDown(Opponent opponent)
        {
            Opponent = opponent;
        }
        protected override void SetUp(Tween gfx)
        {
            var card = Opponent.Hand[0];
            var origin = card.Translation;
            var destination = Opponent.Support.NextSlot() + new Vector3(0, 0, 0.05F);

            Opponent.Hand.Remove(card);
            Opponent.Support.Add(card);

            gfx.InterpolateProperty(card, nameof(card.Translation), origin, destination, 0.3F);
            gfx.InterpolateProperty(card, nameof(card.RotationDegrees), new Vector3(-25, 0, 0), new Vector3(0, 0, 0),
                0.1F);
            gfx.InterpolateCallback(Opponent.Hand, 0.2F, nameof(Hand.Sort));
        }
    }
}