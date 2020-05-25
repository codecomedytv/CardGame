using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {

	public class Game : Node
	{
		private List<Player> Players;
		
		[Signal]
		delegate void GamestateUpdated();
		
		[Signal]
		delegate void Disqualified();
		
		public Game() {}
		
		public Game(List<Player> players)
		{
			Players = players;
		}
	}
}
