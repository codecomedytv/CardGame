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
			//AddChild((Node) cardView);
			var card3D = (Card3DView) cardView;
			card3D.GlobalTransform = GlobalTransform;
			card3D.Position = new Vector3(card3D.Position.x, card3D.Position.y, Cards.Count * 0.01F);
		}
		
		public void Remove(ICardView cardView)
		{
			Cards.Remove(cardView);
			//RemoveChild((Card3DView) cardView);
		}
		
		// AddToTopOfDeck

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
				card3D.Position = new Vector3(card3D.Position.x, card3D.Position.y, i * 0.01F);
				i += 1;
			}

			cardView.Position = new Vector3(cardView.Position.x, cardView.Position.y, i * 0.01F);
		}

		public void Sort()
		{
			throw new System.NotImplementedException();
		}
	}
}
