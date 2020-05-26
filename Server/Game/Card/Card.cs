using Godot;
using System;
using System.Collections.Generic;

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
		public List<Card> Zone; // Might be worth updating
		public List<Decorator> Tags;
		public bool Legal = false;
		public bool Activated = false;
		
		[Signal]
		public delegate void Exit();
		
		public abstract void OnControllerStateChanged(int state, string signal);
		
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
		
		public Godot.Collections.Dictionary Serialize()
		{
			return new Godot.Collections.Dictionary{{"Id", Id}, {"setCode", SetCode}};
		}
		
		public void SetZone(List<Card> newZone)
		{
			Zone = newZone;
			EmitSignal("Exit");
		}
		
		public bool HasTag(int tag)
		{
			// Enums may work differently in C# as actual objects
			foreach(var decorator in Tags){
				if(decorator.Tag == tag){
					return true;
				}
			}
			return false;
		}
		
		public void CreateSkills()
		{
			// We can't use the GDScript Version
			// but
			// We can use lambdas and things for this
			// We could use skill attributes
		}
		
	}
}
