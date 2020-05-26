using System;
using Godot;
using System.Collections.Generic;

namespace CardGame.Server {

	public class Messenger : Node, IMessenger
	{
		[Signal]
		public delegate void Deployed();

		[Signal]
		public delegate void Attacked();

		[Signal]
		public delegate void Activated();

		[Signal]
		public delegate void Targeted();

		[Signal]
		public delegate void EndedTurn();

		[Signal]
		public delegate void PassedPriority();

		[Signal]
		public delegate void PlayerSeated();

		public Messenger()
		{
			Name = "Messenger";
		}

		public void OnPlayExecuted(Player player, System.Object Event)
		{
			
		}

		public void Update(List<Player> players)
		{
			
		}

		public void DisqualifyPlayer(int player, int reason)
		{
			GD.Print(String.Format("Player {0} disqualified because: {1}"), player, reason);
		}

		public void Deploy(int player, int card)
		{
			EmitSignal(nameof(Deployed), player, card);
		}

		public void Attack(int player, int attacker, int defender)
		{
			EmitSignal(nameof(Attacked), player, attacker, defender);
		}

		public void Activate(int player, int card, int skillIndex, List<int> targets)
		{
			EmitSignal(nameof(Activated), player, card, skillIndex, targets);
		}

		public void Target(int player, int target)
		{
			EmitSignal(nameof(Targeted), target);
		}

		public void PassPlay(int player)
		{
			EmitSignal(nameof(PassedPriority), player);
		}

		public void EndTurn(int player)
		{
			EmitSignal(nameof(EndedTurn), player);
		}

		public void SetReady(int player)
		{
			EmitSignal(nameof(PlayerSeated), player);
		}
	}
}
