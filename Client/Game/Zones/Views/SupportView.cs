using System.Collections;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
	public class SupportView : Spatial, IZoneView
	{
		public void Add(Card card) { card.Translation = GetNode<Sprite3D>($"CardSlot{card.ZoneIndex-1}").GlobalTransform.origin; }
	}
}
