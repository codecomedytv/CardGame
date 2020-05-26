using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {

	public class Link : Reference
	{
		private Gamestate Game;
		private List<Skill> Chain = new List<Skill>();
		private List<Skill> Constants = new List<Skill>();
		private List<Skill> Manual = new List<Skill>();
		private List<Skill> Auto = new List<Skill>();
		
		[Signal]
		public delegate void Updated();
		
		public void SetUp(Gamestate game)
		{
			if(Game == null) { Game = game; }
		}
		
		public void AddResolvable(Godot.Object action)
		{
			
		}
		
		public void ApplyConstants(string gameEvent)
		{
			
		}
		
		public void ApplyTriggered(string gameEvent)
		{
			
		}
		
		public void  SetupManual(string gameEvent)
		{
			
		}
		
		public void Broadcast(string gameEvent, List<System.Object> arguments)
		{
			
		}
		
		public void Register(Card card)
		{
			
		}
		
		public void Unregister(Skill skill)
		{
			
		}
		
		public void OnPriorityPassed(int player)
		{
			
		}
		
		public void Activate(Player player, Card card, int skillIndex = 0, List<Godot.Object> args = null)
		{
			
		}
		
		public void Resolve()
		{
			
		}
		
		public void Update()
		{
			
		}
		
	}

}
