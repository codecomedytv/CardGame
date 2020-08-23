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
			AddChild((Node) cardView);
			var card3D = (Card3DView) cardView;
			card3D.Position = new Vector3(0, 0, Cards.Count * 0.01F);
		}
		
		public void Remove(ICardView cardView)
		{
			Cards.Remove(cardView);
			RemoveChild((Card3DView) cardView);
		}

		public void Sort()
		{
			throw new System.NotImplementedException();
		}
	}
}
