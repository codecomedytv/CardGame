using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace CardGame.Server {

	public abstract class Card : Reference
	{
		public string Title = "Card";
		public readonly int SetCode = 0;
		public readonly int Id;
		public Player Owner;
		public Player Controller;
		public Player Opponent;
		public List<Skill> Skills;
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
			Owner = owner;
			foreach(var skill in Skills){
				skill.Owner = Owner;
			}
		}
			
		public void SetControllerAndOpponent(Player controller)
		{
			Controller = controller;
			foreach(var skill in Skills) {
				skill.Controller = controller;
				skill.Opponent = controller.Opponent;
			}
		}
		
		public Dictionary<string, int> Serialize()
		{
			return new Dictionary<string, int>{{"Id", Id}, {"setCode", SetCode}};
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

			//foreach (var skill in GetType().GetNestedTypes().Where(s => s.IsDefined(typeof(SkillAttribute))))
			//{
			//	Skill instance = new skill();
			//	Skills.Add(new skill());
			//}
			throw new NotImplementedException();
		}

		protected virtual void SetAttributes()
		{
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
