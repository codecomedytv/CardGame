using CardGame.Server.Game.Cards;
using Godot;

namespace CardGame.Server.Game {
		
	public class Gamestate : Reference
	{
		private readonly CardCatalog CardCatalog = new CardCatalog();
		public Unit Attacking;

		public void RegisterCard(Card card) => CardCatalog.RegisterCard(card);

		public Card GetCard(int id) => CardCatalog.GetCard(id);
		
	}

}
