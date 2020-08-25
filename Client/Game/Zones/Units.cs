using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
	public class Units: Spatial, IZone
	{
		private IList<Card> Cards = new List<Card>();
		public IEnumerator<Card> GetEnumerator()
		{
			throw new System.NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Vector3 NextSlot()
		{
			return GetNode<Sprite3D>($"CardSlot{Count}").GlobalTransform.origin;
		}

		public int Count => Cards.Count;
		
		public void Add(Card card)
		{
			Cards.Add(card);
		}

		public bool Contains(Card card)
		{
			return Cards.Contains(card);
		}

		public void Remove(Card card)
		{
			Cards.Remove(card);
		}

		public void Sort()
		{
			throw new System.NotImplementedException();
		}
	}
}
