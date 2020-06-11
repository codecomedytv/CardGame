using CardGame.Server.Game.Cards;
using Godot;

namespace CardGame.Server.Game {
		
	public class Gamestate : Reference
	{
		public int NextCardID = 0;
		public Godot.Collections.Dictionary<int, Card> CardCatalog = new Godot.Collections.Dictionary<int, Card>();
		public Unit Attacking;

		public void RegisterCard(Card card)
		{
			card.Id = NextCardID;
			CardCatalog[card.Id] = card;
			NextCardID++;
		}

		public Card GetCard(int id) => CardCatalog[id];
		
	}

}
