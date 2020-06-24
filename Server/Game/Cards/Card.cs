using System.Collections.Generic;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skill;
using CardGame.Server.Game.Zones;
using Godot;

namespace CardGame.Server.Game.Cards {

	public abstract class Card : Reference, ISource
	{
		
		public string Title = "Card";
		public SetCodes SetCode = 0;
		public int Id;
		public Player Owner;
		public Player Controller;
		public Player Opponent => Controller.Opponent;
		public Skill.Skill Skill;
		public Zone Zone;
		
		// When a player enters an active state (idle or active) then it iterates on all
		// owned cards to see if these can be used or not.
		public bool IsReady = false;
		public bool Activated = false;
		public bool CanBeDeployed;
		public bool CanBeSet;
		public bool CanBeActivated = false;
		public bool CanAttack;

		protected Card()
		{
			AddSkill(new NullSkill());
		}
		

		public virtual void SetCanBeDeployed() => CanBeDeployed = false;

		public virtual void SetCanBeSet() => CanBeSet = false;

		public virtual void SetCanAttack() => CanAttack = false;

		public virtual void SetCanBeActivated() => CanBeActivated = false;

		public void Ready() => IsReady = true;

		public void Exhaust() => IsReady = false;

		public Dictionary<string, int> Serialize() => new Dictionary<string, int>{{"Id", Id}, {"setCode", (int)SetCode}};
		
		protected void AddSkill(Skill.Skill skill)
		{
			Skill = skill;
			skill.Card = this;
		}
		
		public override string ToString() => $"{Id.ToString()}: {Title}";

	}
}
