using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

namespace CardGame.Server {
	
	public class Skill : Godot.Object, IResolvable
	{
		public enum Types { Manual, Auto, Constant }
		public Player Owner;
		public Player Controller;
		public Player Opponent;
		public Card Card;
		public bool CanBeUsed = false;
		public string AreaOfActivation;
		public Dictionary Parameters;
		public Gamestate GameState;
		public Types Type = Types.Manual;
		public string GameEvent;

		public void _SetLegal(bool value)
		{
			CanBeUsed = value;
			if (value)
			{
				Card.SetLegal();
			}
			else
			{
				Card.SetIllegal();
			}
		}

		public void SetUp(string gameEvent)
		{
			if (!GameEvent.Empty() && GameEvent != gameEvent)
			{
				return;
			}


			_SetUp();
			if(CanBeUsed && Card is Support support)
			{
				support.CanBeActivated = true;
			}
		}

		public virtual void _SetUp()
		{
			// This is accessed with self but there is no clear setget in the source?
			CanBeUsed = true;
		}

		public void Activate()
		{
			Card.Activated = true;
			CanBeUsed = false;
			// _SetLegal(false)
			_Activate();
		}

		public void Resolve(string gameEvent = "")
		{
			if (!GameEvent.Empty() && GameEvent != gameEvent && Type == Types.Constant)
			{
				return;
			}

			_Resolve();

			if (Card is Support)
			{
				Controller.Support.Remove(Card);
				Owner.Graveyard.Add(Card);
			}
		}

		public virtual void _Activate()
		{
		}

		public virtual void _Resolve()
		{
		}

		public void AddTagToController(Tag tag)
		{
			var decorator = new Decorator(tag);
			decorator.AddTagToPlayer(Controller);
		}

		public void AddTagToGroup(List<Card> cards, Tag tag, string destroyCondition, string taggedExit, bool includeCard = true)
		{
			var decorator = new Decorator(tag);
			if (destroyCondition != "")
			{
				Card.Connect(destroyCondition, decorator, nameof(decorator.OnZoneExit),
					new Array {Card.Zone, Card, "Destroy"});
			}

			foreach (var card in cards)
			{
				if (Card == card && !includeCard)
				{
					// I have no idea what this is doing?
					continue;
				}
				decorator.AddTagTo(card);
				if (taggedExit != "")
				{
					Card.Connect(taggedExit, decorator,
						nameof(decorator.OnZoneExit), new Array {Card.Zone, Card, "unTag"});
				}
			}
		}

		public void SetTargets(List<Card> cards)
		{
			Controller.DeclarePlay(new SetTargets(Card, cards));
		}

		public void AutoTarget()
		{
			Controller.EmitSignal(nameof(Player.Pause));
			Controller.DeclarePlay(new AutoTarget(Card));
		}
	}
	
}
