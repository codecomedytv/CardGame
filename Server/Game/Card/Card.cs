using Godot;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace CardGame.Server {

	public abstract class Card : Reference
	{
		public string Title = "Card";
		public SetCodes SetCode = 0;
		public int Id;
		public Player Owner;
		public Player Controller;
		public Player Opponent => Controller.Opponent;
		public Skill Skill;
		public List<Card> Zone; //= new List<Card> // Might be worth updating
		public List<Decorator> Tags = new List<Decorator>();
		public bool Activated = false;
		public bool Ready = false;
		// public bool Attacked = false;

		// When a player enters an active state (idle or active) then it iterates on all
		// owned cards to see if these can be used or not.
		public bool CanBeDeployed = false;
		public bool CanBeSet = false;
		public bool CanBeActivated = false;
		public bool CanAttack = false;

		public Card()
		{
			AddSkill(new NullSkill());
		}

		public virtual void SetCanBeDeployed()
		{
			CanBeDeployed = false;
		}

		public virtual void SetCanBeSet()
		{
			CanBeSet = false;
		}

		public virtual void SetCanAttack()
		{
			CanAttack = false;
		}

		public virtual void SetCanBeActivated()
		{
			CanBeActivated = false;
		}
		

		[Signal]
		public delegate void Exit();
		
		public Dictionary<string, int> Serialize()
		{
			return new Dictionary<string, int>{{"Id", Id}, {"setCode", (int)SetCode}};
		}
		
		public void SetZone(List<Card> newZone)
		{
			Zone = newZone;
			EmitSignal(nameof(Exit));
		}
		
		public bool HasTag(Tag tag)
		{
			foreach(var decorator in Tags){
				if(decorator.Tag == tag){
					return true;
				}
			}
			return false;
		}

		protected void AddSkill(Skill skill)
		{
			Skill = skill;
			skill.Card = this;
		}

		protected void SetSkillCards()
		{
			Skill.Card = this;
		}

		public override string ToString()
		{
			return String.Format("{0}: {1}", Id.ToString(), Title);
		}

	}
}
