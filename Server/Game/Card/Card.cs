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
		public List<Decorator> Tags;
		public bool Legal = false;
		public bool Activated = false;
		public bool Ready = false;


		[AttributeUsage(AttributeTargets.Class)]
		protected class SkillAttribute : System.Attribute
		{
		}

		[Signal]
		public delegate void Exit();

		public virtual void OnControllerStateChanged(int state, string signal)
		{
			
		}
		
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
			//foreach (var skill in GetType().GetNestedTypes().Where(s => s == Skill))
			//{
			//	var  instance = new skill;
			//	Skills.Add(instance);
			//}
		}

		protected void AddSkill(Skill skill)
		{
			skill.Card = this;
			Skills.Add(skill);
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
			_SetAsPlayable();
		}

		protected virtual void _SetAsPlayable()
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
