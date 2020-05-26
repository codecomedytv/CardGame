using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {
	
	public class Decorator : Node
	{
		public Tag Tag;
		public List<Card> Decorated = new List<Card>();
	}
}
