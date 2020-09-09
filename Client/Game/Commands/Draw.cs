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
			Card.Controller.Deck.Add(Card);
			var globalPosition = Card.Translation;
			Card.Controller.Deck.Remove(Card);
			Card.Controller.Hand.Add(Card);
			var globalDestination = Card.Translation;
			var rotation = new Vector3(-25, 180, 0);

			// Wrap In gfx Class
			gfx.InterpolateProperty(Card, nameof(Card.Visible), false, true, 0.1F);
			gfx.InterpolateProperty(Card, nameof(Card.Translation), globalPosition, globalDestination, 0.1F);
			gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), Card.Rotation, rotation, 0.1F);
		}
	}
}
