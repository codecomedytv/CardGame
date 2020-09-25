using System;
using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Zones;
using Godot;

namespace CardGame.Client.Game
{
	public class Draw : Command
	{
		private readonly Card Card;

		public Draw(Card card)
		{
			Card = card;
		}

		protected override void SetUp(Effects gfx)
		{
			Card.Visible = false;
			Card.Controller.Deck.Remove(Card);
			Card.Controller.Deck.Add(Card);

			var globalPosition = Card.Translation;
			Card.Controller.Deck.Remove(Card);
			Card.Controller.Hand.Add(Card);
			
			Card.Translation = Card.Controller.View.Hand.GlobalTransform.origin;
			var sorter = new Sorter(Card.Controller.Hand);
			sorter.Sort();
			
			// Our destination is where the card is POST-SORT, not the hand origin itself
			var globalDestination = Card.Translation;
			
			var rotation = new Vector3(-25, 180, 0);

			// Wrap In gfx Class
			gfx.Play(Audio.Draw);
			Card.Translation = globalPosition;
			gfx.InterpolateProperty(Card, "translation", Card.Translation, globalPosition, 0.09F);
			gfx.InterpolateProperty(Card, "visible", false, true, 0.1F);
			gfx.InterpolateProperty(Card, "translation", globalPosition, globalDestination, 0.2F, delay: 0.1F);
			gfx.InterpolateProperty(Card, "rotation_degrees", Card.Rotation, rotation, 0.2F, delay: 0);
		}
	}
}
