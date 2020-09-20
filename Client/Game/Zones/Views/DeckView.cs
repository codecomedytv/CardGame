using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Zones
{
	public class DeckView: Sprite3D, IZoneView
	{
		
		public void Add(Card card)
		{
			card.Translation = GlobalTransform.origin;
			card.Translation = new Vector3(card.Translation.x, card.Translation.y, card.ZoneIndex * 0.01F);
		}
		
	}
}
