using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {

	public class Game : Node
	{
		private List<Player> Players;
		
		public Game() {}
		
		public Game(List<Player> players)
		{
			Players = players;
		}
	}
}
