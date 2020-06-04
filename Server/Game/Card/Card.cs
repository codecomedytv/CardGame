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
		public Player Opponent;
		public List<Skill> Skills = new List<Skill>();
		public List<Card> Zone; //= new List<Card> // Might be worth updating
		public List<Decorator> Tags = new List<Decorator>();
		public bool Legal = false;
		public bool Activated = false;
		public bool Ready = false;
		// public bool Attacked = false;

		// The intention is for these to be set throughout the turn but never declared
		// When a player enters an active state (idle or active) then it iterates on all
		// owned cards to see if these can be used or not.
		public bool CanBeDeployed = false;
		public bool CanBeSet = false;
		public bool CanBeActivated = false;
		public bool CanAttack = false;

		public virtual void SetCanBeDeployed()
		{
			CanBeDeployed = false;
		}

		public virtual void SetCanBeSet()
		{
			CanBeSet = false;
		}


		[AttributeUsage(AttributeTargets.Class)]
		protected class SkillAttribute : System.Attribute
		{
		}

		[Signal]
		public delegate void Exit();
		
		public void SetOwner(Player owner) 
		{
			// This is a setget method on original source code;
			Owner = owner;
			foreach(var skill in Skills){
				skill.Owner = Owner;
			}
		}
			
		public void SetControllerAndOpponent(Player controller)
		{
			// This is a setget method on Controller .property from original source code
			Controller = controller;
			Opponent = controller.Opponent;
			foreach(var skill in Skills) {
				skill.Controller = controller;
				skill.Opponent = controller.Opponent;
			}
		}
		
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
		
		protected void CreateSkills()
		{

			GD.PushWarning("Add Skills Manually");
		}

		protected void AddSkill(Skill skill)
		{
			skill.Card = this;
			Skills.Add(skill);
			skill.Controller = Controller;
		}

		protected void SetSkillCards()
		{
			foreach (var skill in Skills)
			{
				skill.Card = this;
			}
		}

		public void SetAsPlayable()
		{
		}
		
		public void SetLegal()
		{
			Legal = true;
			Controller.SetLegal(this);
		}

		public void SetIllegal()
		{
			Legal = false;
			Controller.Forbid(this);
		}

		public override string ToString()
		{
			return String.Format("{0}: {1}", Id.ToString(), Title);
		}
	}
}
