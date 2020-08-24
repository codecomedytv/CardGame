using System.Collections;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones.Zones3D
{
	public class Deck3D: Sprite3D, IZoneView
	{
		private readonly IList<ICardView> Cards = new List<ICardView>();
		
		public void Add(ICardView card)
		{
			if(Cards.Contains(card))
			{
				AddToTopOfDeck(card);
				return;
			}
			Cards.Add(card);
			card.Position = GlobalTransform.origin;
			card.Position = new Vector3(card.Position.x, card.Position.y, Cards.Count * 0.01F);
		}
		
		public void Remove(ICardView cardView)
		{
			Cards.Remove(cardView);
		}
		
		private void AddToTopOfDeck(ICardView addedCard)
		{
			var i = 0;
			foreach (var card in Cards)
			{
				if (card == addedCard)
				{
					continue;
				}

				card.Position = new Vector3(card.Position.x, card.Position.y, i * 0.01F);
				i += 1;
			}

			addedCard.Position = new Vector3(addedCard.Position.x, addedCard.Position.y, i * 0.01F);
		}
		
		public void Sort()
		{
			throw new System.NotImplementedException();
		}

		public IEnumerator<ICardView> GetEnumerator()
		{
			return Cards.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
