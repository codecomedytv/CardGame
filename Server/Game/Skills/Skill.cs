using System;
using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Zones;
using Godot;

namespace CardGame.Server.Game.Skills {
	
	public partial class Skill : Godot.Object, IResolvable
	{
		public enum Types { Manual, Auto, Constant }

		[Signal]
		public delegate void Resolved();

		private Player Owner => Card.Owner;
		public Player Controller => Card.Controller;
		protected Player Opponent => Card.Opponent;
		public Card Card;
		public bool CanBeUsed = false;
		public Match Match;
		public History History => Match.History;
		public Types Type = Types.Manual;
		protected readonly List<GameEvents> Triggers = new List<GameEvents>();
		protected readonly List<Zone> AreaOfEffects = new List<Zone>();
		public Card Target;
		public readonly List<Card> ValidTargets = new List<Card>();
		public bool Targeting = false;
		
		public void Resolve()
		{
			_Resolve();
			Card.Activated = false;
			if (!(Card is Support)) return;
			Controller.Support.Remove(Card);
			Owner.Graveyard.Add(Card);
			EmitSignal(nameof(Resolved));
		}
		
		protected virtual void _Resolve()
		{
		}


		
	}
	
}
