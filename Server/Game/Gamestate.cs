using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CardGame.Server {
		
	public class Gamestate : Reference
	{
		public int NextCardID = 0;
		public Godot.Collections.Dictionary<int, Card> CardCatalog = new Godot.Collections.Dictionary<int, Card>();
		public Unit Attacking;
		public Player TurnPlayer;

		public void RegisterCard(Card card)
		{
			card.Id = NextCardID;
			CardCatalog[card.Id] = card;
			NextCardID++;
		}

		public Card GetCard(int id) => CardCatalog[id];
		
		public Player GetTurnPlayer() => TurnPlayer;

	}

}
