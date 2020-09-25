using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game
{
	public class SetFaceDown: Command
	{
		private readonly Card Card;

		public SetFaceDown(Card card)
		{
			Card = card;
		}
		protected override void SetUp(Effects gfx)
		{
			var origin = Card.Translation;

			Card.Controller.Hand.Remove(Card);
			Card.Controller.Support.Add(Card);
			
			var destination = Card.Controller.View.Support.GetNode<Sprite3D>($"CardSlot{Card.ZoneIndex - 1}").GlobalTransform.origin;
			destination += new Vector3(0, 0, 0.05F);
			Card.Translation = origin;
			
			gfx.Play(Audio.SetCard);
			gfx.InterpolateProperty(Card, "translation", origin, destination, 0.3F);
			gfx.InterpolateProperty(Card, "rotation_degrees", new Vector3(-25, 180, 0), new Vector3(0, 0, 0), 0.1F);
			gfx.InterpolateCallback(new Sorter(Card.Controller.Hand), 0.2F, nameof(Sorter.Sort));
		}
	}
}
