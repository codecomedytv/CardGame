using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {

	public class Player : Node
	{
		private List<System.Object> Decklist;
		public readonly int ID;
		
		public Player() {}
		
		public Player(int id, List<System.Object> decklist)
		{
			Decklist = decklist;
			ID = id;
		}
	}
}
