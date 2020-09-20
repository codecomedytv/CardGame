using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game
{
	public class Draw: Command
	{
		private readonly Card Card;

		public Draw(Card card)
		{
			Card = card;
		}
		protected override void SetUp(Effects gfx)
		{
			Card.Visible = false;
			Card.Controller.DeckModel.Remove(Card);
			Card.Controller.DeckModel.Add(Card);
			var globalPosition = Card.Translation;
			Card.Controller.DeckModel.Remove(Card);
			Card.Controller.HandModel.Add(Card);
			var globalDestination = Card.Translation;
			var rotation = new Vector3(-25, 180, 0);

			// Wrap In gfx Class
			gfx.Play(Audio.Draw);
			Card.Translation = globalPosition;
			gfx.InterpolateProperty(Card, nameof(Card.Translation), Card.Translation, globalPosition, 0.09F);
			gfx.InterpolateProperty(Card, nameof(Card.Visible), false, true, 0.1F);
			gfx.InterpolateProperty(Card, nameof(Card.Translation), globalPosition, globalDestination, 0.2F, delay: 0.1F);
			gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), Card.Rotation, rotation, 0.2F, delay: 0);
		}
	}
}
