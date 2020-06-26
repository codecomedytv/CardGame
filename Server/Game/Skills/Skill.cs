using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;

namespace CardGame.Server.Game.Skills {
	
	public partial class Skill : Godot.Object, IResolvable
	{
		public enum Types { Manual, Auto, Constant }

		private Player Owner => Card.Owner;
		public Player Controller => Card.Controller;
		protected Player Opponent => Card.Opponent;
		public Card Card;
		public bool CanBeUsed = false;
		public Match Match;
		public History History => Match.History;
		public Types Type = Types.Manual;
		protected readonly List<GameEvents> Triggers = new List<GameEvents>();
		public Card Target;
		public bool Targeting = false;

		public void SetUp(Command gameEvent)
		{
			if (Triggers.Count > 0 && !Triggers.Contains(gameEvent.Identity))
			{
				return;
			}


			_SetUp();
			if(CanBeUsed && Card is Support support)
			{
				support.CanBeActivated = true;
			}
		}

		protected virtual void _SetUp()
		{
			CanBeUsed = true;
		}

		public void Activate()
		{
			Card.Activated = true;
			CanBeUsed = false;
			_Activate();
		}

		public void Resolve()
		{
			// if (GameEvent != GameEvents.NoOp && GameEvent != gameEvent.Identity && Type == Types.Constant)
			// {
			// 	return;
			// }
			//
			
			_Resolve();
			Card.Activated = false;
			if (!(Card is Support)) return;
			Controller.Support.Remove(Card);
			Owner.Graveyard.Add(Card);
		}

		protected virtual void _Activate()
		{
		}

		protected virtual void _Resolve()
		{
		}


		
	}
	
}
