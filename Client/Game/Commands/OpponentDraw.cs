using System.Linq;
using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class OpponentDraw: Command
    {
        private readonly Opponent Opponent;

        public OpponentDraw(Opponent opponent)
        {
            Opponent = opponent;
        }
        protected override void SetUp(Effects gfx)
        {
            GD.Print(Opponent.DeckModel.Count);
            var card = Opponent.DeckModel.Last();
            //Opponent.DeckModel.Add(card); // We keep re-adding the card? Does C# have a UniqueCollection?
            var globalPosition = card.Translation;
            Opponent.DeckModel.Remove(card);
            Opponent.Hand.Add(card);
            var globalDestination = card.Translation;
            var rotation = new Vector3(60, 0, 0);
				
            gfx.Play(Audio.Draw);
            gfx.InterpolateProperty(card, nameof(card.Translation), card.Translation, globalPosition, 0.09F);
            gfx.InterpolateProperty(card, nameof(card.Visible), false, true, 0.1F);
            gfx.InterpolateProperty(card, nameof(card.Translation), globalPosition, globalDestination, 0.2F, delay: 0.1F);
            gfx.InterpolateProperty(card, nameof(card.RotationDegrees), card.Rotation, rotation, 0.2F, delay: 0);
        }
    }
}