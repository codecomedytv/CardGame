using System.Collections;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
	public class Deck: Sprite3D, IZone
	{
		private readonly IList<Card> Cards = new List<Card>();
		
		public int Count => Cards.Count;
		public void Add(Card card)
		{
			Cards.Add(card);
			card.Translation = GlobalTransform.origin;
			card.Translation = new Vector3(card.Translation.x, card.Translation.y, Cards.Count * 0.01F);
		}
		
		public void Remove(Card card)
		{
			Cards.Remove(card);
		}

		public void Sort()
		{
			throw new System.NotImplementedException();
		}

		public IEnumerator<Card> GetEnumerator()
		{
			return Cards.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
