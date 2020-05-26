using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {

	public class Player : Node
	{
		private List<Godot.Object> DeckList;
		public readonly int Id;
		public Player Opponent;
		
		[Signal]
		public delegate void PlayExecuted();
		
		[Signal]
		public delegate void PriorityPassed();

		[Signal]
		public delegate void TurnEnded();

		[Signal]
		public delegate void Register();

		[Signal]
		public delegate void Deployed();

		[Signal]
		public delegate void Paused();
		

		public Player() {}
		
		public Player(int id, List<Godot.Object> deckList)
		{
			DeckList = deckList;
			Id = id;
		}
	}
}
