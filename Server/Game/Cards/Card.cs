using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Skills;
using CardGame.Server.Game.Tags;
using CardGame.Server.Game.Zones;
using Godot;

namespace CardGame.Server.Game.Cards {

	public abstract class Card : Reference, ISource
	{
		protected string Title = "Card";
		protected SetCodes SetCode = 0;
		public int Id;
		public Player Owner;
		public Player Controller;
		public Player Opponent => Controller.Opponent;
		public Skill Skill;
		public Zone Zone;

		// When a player enters an active state (idle or active) then it iterates on all
		// owned cards to see if these can be used or not.
		public History History => Controller.Match.History;

		public bool IsReady = false;
		public bool Activated = false;
		public bool CanBeDeployed;
		public bool CanBeSet;
		public bool CanBeActivated = false;
		public bool CanAttack;
		public readonly List<Tag> Tags = new List<Tag>();

		protected Card()
		{
			Skill = new NullSkill {Card = this};
		}
		

		public virtual void SetCanBeDeployed() => CanBeDeployed = false;

		public virtual void SetCanBeSet() => CanBeSet = false;

		public virtual void SetCanAttack() => CanAttack = false;

		public virtual void SetCanBeActivated() => CanBeActivated = false;

		public void Ready() => IsReady = true;

		public void Exhaust() => IsReady = false;

		public bool HasTag(TagIds tagId) => Tags.Any(tag => tag.TagId == tagId);

		public Dictionary<string, int> Serialize() => new Dictionary<string, int>{{"Id", Id}, {"setCode", (int)SetCode}};

		public override string ToString() => $"{Id.ToString()}: {Title}";

	}
}
