using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Cards.Card3D;
using Godot;

namespace CardGame.Client.Game.Zones.Zones3D
{
	public class Deck3D: Sprite3D, IZoneView
	{
		private readonly List<ICardView> Cards = new List<ICardView>();
		
		public void Add(ICardView cardView)
		{
			Cards.Add(cardView);
			var card3D = (Card3DView) cardView;
			card3D.Translation = GlobalTransform.origin;
			card3D.Translation = new Vector3(card3D.Translation.x, card3D.Translation.y, Cards.Count * 0.01F);
		}
		
		public void Remove(ICardView cardView)
		{
			Cards.Remove(cardView);
		}
		
		public void AddToTopOfDeck(Card3DView cardView)
		{
			var i = 0;
			foreach (var card in Cards)
			{
				if (card == cardView)
				{
					continue;
				}

				var card3D = (Card3DView) card;
				card3D.Translation = new Vector3(card3D.Translation.x, card3D.Translation.y, i * 0.01F);
				i += 1;
			}

			cardView.Translation = new Vector3(cardView.Translation.x, cardView.Translation.y, i * 0.01F);
		}

		public void Sort()
		{
			throw new System.NotImplementedException();
		}
	}
}
