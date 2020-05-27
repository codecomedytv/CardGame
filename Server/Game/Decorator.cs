using Godot;
using System;
using System.Collections.Generic;
using Object = Godot.Object;

namespace CardGame.Server {
	
	public class Decorator : Node
	{
		public readonly Tag Tag;
		public List<Card> Decorated = new List<Card>();

		public Decorator(Tag tag)
		{
			Tag = tag;
		}

		public void AddTagTo(Object tagged)
		{
			throw new NotImplementedException();
		}

		public void OnZoneExit(List<Card> zone, Card card, string todo)
		{
			
		}
	}
}
