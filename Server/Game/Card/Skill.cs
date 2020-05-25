using Godot;
using System;

namespace CardGame.Server {
	
	public class Skill : Godot.Object
	{
		public Player Owner;
		public Player Controller;
		public Player Opponent;
	}
	
}
