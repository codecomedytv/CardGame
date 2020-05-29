using Godot;
using System;
using System.Collections.Generic;
using Object = Godot.Object;

namespace CardGame.Server {
	
	public class Decorator : Node
	{
		public readonly Tag Tag;
		public List<Object> Decorated = new List<Object>();

		public Decorator()
		{
			
		}
		public Decorator(Tag tag)
		{
			Tag = tag;
		}

		public void AddTagTo(Card tagged)
		{
			if (Decorated.Contains(tagged))
			{
				return;
			}
			Decorated.Add(tagged);
			tagged.Tags.Add(this);
		}

		public void UnTag(Card untagged)
		{
			if (!Decorated.Contains(untagged))
			{
				return;
			}

			Decorated.Remove(untagged);
			untagged.Tags.Remove(this);
		}

		public void AddTagToPlayer(Player tagged)
		{
			if (Decorated.Contains(tagged))
			{
				return;
			}
			Decorated.Add(tagged);
			tagged.Tags.Add(this);
		}

		private void Destroy()
		{
			while (Decorated.Count != 0)
			{
				if (Decorated[0] is Card card)
				{
					UnTag(card);
				}
				else
				{
					GD.PushWarning("Untagging Players Not Implemented!");
				}
			}
		}

		public void OnZoneExit(List<Card> zone, Card card, string command)
		{
			GD.Print("command");
			if(zone == card.Zone)
			{
				// Not a New Zone
				return;
			}

			switch (command)
			{
				case "destroy":
					Destroy();
					break;
				case "untag":
					UnTag(card);
					break;
				default:
					throw new Exception("Unreachable Reached");
			}
		}
	}
}
