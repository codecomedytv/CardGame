using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using CardGame.Client.Game.Zones.ModelControllers;
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

			Card.Controller.HandModel.Remove(Card);
			Card.Controller.SupportModel.Add(Card);
			var destination = Card.Translation + new Vector3(0, 0, 0.05F);
			Card.Translation = origin;
			
			gfx.Play(Audio.SetCard);
			gfx.InterpolateProperty(Card, nameof(Card.Translation), origin, destination, 0.3F);
			gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), new Vector3(-25, 180, 0), new Vector3(0, 0, 0), 0.1F);
			gfx.InterpolateCallback(new Deploy.HelperNode(Card.Controller.HandModel), 0.2F, nameof(Deploy.HelperNode.Sort));
		}
		
		public class HelperNode: Node
		{
			private IZoneModelController Zone;
			public HelperNode(IZoneModelController zone)
			{
				Zone = zone;
			}

			public void Sort()
			{
				Zone.Sort();
				QueueFree();
			}
		}
	}
}
