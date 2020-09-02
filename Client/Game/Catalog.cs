using System.Collections;
using System.Collections.Generic;
using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game
{
	public class Catalog: IEnumerable<Card>
	{
		
		private readonly Dictionary<int, Card> Cards = new Dictionary<int, Card>();
		public void Add(int id, Card card) => Cards[id] = card;
		public Card this[int key] => Cards[key];
		public IEnumerator<Card> GetEnumerator() => Cards.Values.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
