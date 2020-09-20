using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
	public class UnitsView: Spatial, IZoneView
	{

		private IList<Card> Cards = new List<Card>();
		public int Count => Cards.Count;

		public void Add(Card card)
		{
			Cards.Add(card);
			card.Translation = GetNode<Sprite3D>($"CardSlot{Count-1}").GlobalTransform.origin;
		}

		public void Remove(Card card)
		{
			Cards.Remove(card);
		}
		public void Sort() { throw new System.NotImplementedException(); }
	}
}
