using System.Linq;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class OpponentDraw: xCommand
    {
        private readonly Opponent Opponent;

        public OpponentDraw(Opponent opponent)
        {
            Opponent = opponent;
        }
        protected override void SetUp(Tween gfx)
        {
            var card = Opponent.Deck.ElementAt(Opponent.Deck.Count - 1);
            Opponent.Deck.Add(card);
            var globalPosition = card.Translation;
            Opponent.Deck.Remove(card);
            Opponent.Hand.Add(card);
            var globalDestination = card.Translation;
            var rotation = new Vector3(60, 0, 0);
				
            gfx.InterpolateProperty(card, nameof(card.Visible), false, true, 0.1F);
            gfx.InterpolateProperty(card, nameof(card.Translation), globalPosition, globalDestination, 0.1F);
            gfx.InterpolateProperty(card, nameof(card.RotationDegrees), card.Rotation, rotation, 0.1F);
        }
    }
}