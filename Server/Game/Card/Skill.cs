using Godot;
using System;

namespace CardGame.Server {
	
	public class Skill : Godot.Object
	{
		public Player Owner;
		public Player Controller;
		public Player Opponent;
		public Card Card;
		public bool CanBeUsed = false;

		public void SetUp(string gameEvent)
		{
			throw new NotImplementedException();
		}
	}
	
}
