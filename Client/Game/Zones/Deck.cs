using System.Collections;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
	public class Deck: Sprite3D, IZoneView
	{
		private readonly IList<Card> Cards = new List<Card>();
		
		public int Count => Cards.Count;
		public void Add(Card card)
		{
			if(Cards.Contains(card))
			{
				AddToTopOfDeck(card);
				return;
			}
			Cards.Add(card);
			card.Translation = GlobalTransform.origin;
			card.Translation = new Vector3(card.Translation.x, card.Translation.y, Cards.Count * 0.01F);
		}
		
		public void Remove(Card card)
		{
			Cards.Remove(card);
		}
		
		private void AddToTopOfDeck(Card addedCard)
		{
			var i = 0;
			foreach (var card in Cards)
			{
				if (card == addedCard)
				{
					continue;
				}

				card.Translation = new Vector3(card.Translation.x, card.Translation.y, i * 0.01F);
				i += 1;
			}

			addedCard.Translation = new Vector3(addedCard.Translation.x, addedCard.Translation.y, i * 0.01F);
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
