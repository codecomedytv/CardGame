using System.Linq;
using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
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
            var card = Opponent.Deck.Last();
            var globalPosition = card.Translation;
            Opponent.Deck.Remove(card);
            Opponent.Hand.Add(card);
            
            card.Translation = card.Controller.Hand.View.GlobalTransform.origin;
            var sorter = new Sorter(card.Controller.Hand);
            sorter.Sort();
			
            // Our destination is where the card is POST-SORT, not the hand origin itself
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