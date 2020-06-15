using Godot;
using Godot.Collections;
using Card = CardGame.Client.Library.Card.Card;
using Zone = System.Collections.Generic.List<CardGame.Client.Library.Card.Card>;
using Array = Godot.Collections.Array;

namespace CardGame.Client.Match.Model
{
	public class Opponent : Reference
	{
		public int Health = 8000;
		public int DeckSize = 0;
		public int HandSize = 0;
		public Zone Field = new Zone();
		public Zone Support = new Zone();
		public Zone Graveyard = new Zone();
		public CardGame.Client.Match.View.Opponent Visual;
		public Dictionary<int, Card> Cards;
		public Player Enemy;
		public Zone Link;

		protected Opponent()
		{
			
		}
		public Opponent(Dictionary<int, Card> cards)
		{
			Cards = cards;
		}

		public void Draw()
		{
			HandSize += 1;
			DeckSize -= 1;
			Visual.Draw(1, DeckSize);
		}
		
		public void LoadDeck(int deckSize)
		{
			DeckSize = deckSize;
			Visual.LoadDeck(deckSize);
		}
	}
}

