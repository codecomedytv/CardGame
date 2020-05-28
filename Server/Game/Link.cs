using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {

	public class Link : Reference
	{
		public Gamestate Game;
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
		
		public void ApplyConstants(string gameEvent = "")
		{
			
		}
		
		public void ApplyTriggered(string gameEvent)
		{
			
		}
		
		public void  SetupManual(string gameEvent)
		{
			
		}
		
		public void Broadcast(string gameEvent, List<Godot.Object> arguments)
		{
			
		}
		
		public void Register(Card card)
		{
			
		}
		
		public void Unregister(Skill skill)
		{
			
		}
		
		public void OnPriorityPassed(int playerId)
		{
			GD.Print("Passing Priority");
			var player = Game.Player(playerId);
			player.State = Player.States.Passing;
			if(player.Opponent.State == Player.States.Passing)
			{
				Resolve();
			}
			else
			{
				player.Opponent.State = Player.States.Active;
			}
			Update();
			

		}
		
		public void Activate(Player player, Card card, int skillIndex = 0, List<int> args = null)
		{
			
		}
		
		public void Resolve()
		{
			while (Chain.Count != 0)
			{
				var skill = Chain[Chain.Count - 1];
				Chain.RemoveAt(Chain.Count - 1);
				skill.Resolve();
			}
			
			ApplyConstants();
			Game.TurnPlayer.State = Player.States.Idle;
			Game.TurnPlayer.Opponent.State = Player.States.Passive;
		}
		
		public void Update()
		{
			EmitSignal(nameof(Updated));
		}
		
	}

}
