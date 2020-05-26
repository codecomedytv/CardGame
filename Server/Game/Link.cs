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
		delegate void Update();
		
		public void SetUp(Gamestate game)
		{
			if(Game == null) { Game = game; }
		}
		
		public void AddResolvable(Godot.Object Action)
		{
			
		}
		
		public void ApplyConstants(String Event)
		{
			
		}
		
		public void ApplyTriggered(String Event)
		{
			
		}
		
		public void  SetupManual(String Event)
		{
			
		}
		
		public void Broadcast(String Event, List<System.Object> Arguments = default(List<System.Object>))
		{
			
		}
		
		public void Register(Card card)
		{
			
		}
		
		public void Unregister(Skill skill)
		{
			
		}
		
		public void OnPriorityPassed(int PlayerID)
		{
			
		}
		
		public void Activate(Player player, Card card, int SkillIndex= 0, List<System.Object> Arguments = default(List<System.Object>))
		{
			
		}
		
		public void Resolve()
		{
			
		}
		
		public void update()
		{
			
		}
	}

}
