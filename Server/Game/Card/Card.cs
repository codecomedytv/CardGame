using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {

	public abstract class Card : Reference
	{
		public String Title = "Card";
		public int Setcode = 0;
		public readonly int ID;
		public Player Owner;
		public Player Controller;
		public Player Opponent;
		public List<Skill> Skills;
		public List<Card> Zone; // Might be worth updating
		public List<Decorator> Tags;
		public bool Legal = false;
		public bool Activated = false;
		
		[Signal]
		public delegate void exit();
		
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
			Godot.Collections.Dictionary data = new Godot.Collections.Dictionary();
			data["id"] = ID;
			data["setcode"] = Setcode;
			return data;
		}
		
		public void SetZone(List<Card> NewZone)
		{
			Zone = NewZone;
			EmitSignal("Exit");
		}
		
		public bool HasTag(int Tag)
		{
			// Enums may work differently in C# as actual objects
			foreach(var decorator in Tags){
				if(decorator.Tag == Tag){
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
		}
		
	}
}
