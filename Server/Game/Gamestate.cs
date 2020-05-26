using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {
		
	public class Gamestate : Reference
	{
		[Signal]
		delegate void RequestTargets();
		
		[Signal]
		delegate void UnPaused();
		
		public System.Object Target;
		public bool Paused = false;
		public Reference History;
		public int NextCardID = 0;
		public Godot.Collections.Dictionary<int, Player>  PlayerMap = new Godot.Collections.Dictionary<int, Player>();
		public List<Player> Players = new List<Player>();
		public Godot.Collections.Dictionary<int, Card> CardCatalog = new Godot.Collections.Dictionary<int, Card>();
		public Card Attacking;
		public Player TurnPlayer;
		public Player ActivePlayer;
	}

}
