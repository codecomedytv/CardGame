using Godot;
using System;
using System.Collections.Generic;
using CardGame.Server.Game;
using CardGame.Server.Game.Cards;
using Godot.Collections;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

namespace CardGame.Server {
	
	public partial class Skill : Godot.Object, IResolvable
	{
		public enum Types { Manual, Auto, Constant }

		public Player Owner => Card.Owner;
		public Player Controller => Card.Controller;
		public Player Opponent => Card.Opponent;
		public Card Card;
		public bool CanBeUsed = false;
		public string AreaOfActivation;
		public Dictionary Parameters;
		public Gamestate GameState;
		public Types Type = Types.Manual;
		public string GameEvent;
		public Card Target;
		public bool Targeting = false;

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
			Card.Activated = false;
			if (Card is Support)
			{
				Controller.Support.Remove(Card);
				Owner.Graveyard.Add(Card);
			}
		}

		protected virtual void _Activate()
		{
		}

		protected virtual void _Resolve()
		{
		}


		
	}
	
}
