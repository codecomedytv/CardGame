using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Skills;
using CardGame.Server.Game.Tags;
using CardGame.Server.Game.Zones;
using Godot;
using Godot.Collections;

namespace CardGame.Server.Game.Cards {

	public abstract class Card : Reference, ISource
	{
		protected string Title = "Card";
		public SetCodes SetCode = 0;
		public int Id;
		public Player Owner;
		public Player Controller;
		public Player Opponent => Controller.Opponent;
		public Skill Skill;
		public Zone Zone;

		// When a player enters an active state (idle or active) then it iterates on all
		// owned cards to see if these can be used or not.
		public History History;
		public bool IsReady = false;
		public States State = States.Passive;
		public bool CanAttackDirectly = false;
		public enum States
		{
			Passive,
			CanBeDeployed,
			CanBeSet,
			CanBeActivated,
			CanAttack,
			CanAttackDirectly,
			Activated
		}

		public readonly List<Tag> Tags = new List<Tag>();

		protected Card()
		{
			Skill = new NullSkill {Card = this};
		}

		// Maybe this should default to int?
		private Godot.Collections.Array<int> GetValidTargets()
		{
			var targets = new Array<int>();
			foreach (var card in Skill.ValidTargets)
			{
				targets.Add(card.Id);
			}

			return targets;
		}

		protected virtual Array<int> GetValidAttackTargets() => new Array<int>();

		public virtual void SetState() => State = States.Passive;

		public void Ready() => IsReady = true;

		public void Exhaust() => IsReady = false;

		public bool HasTag(TagIds tagId) => Tags.Any(tag => tag.TagId == tagId);

		public System.Collections.Generic.Dictionary<string, int> Serialize() => new System.Collections.Generic.Dictionary<string, int>{{"Id", Id}, {"setCode", (int)SetCode}};

		public void Update(Message message)
		{
			message(Controller.Id, CommandId.UpdateCard, Id, State, GetValidAttackTargets(), GetValidTargets());
		}

		public override string ToString() => $"{Id.ToString()}: {Title}";

	}
}
