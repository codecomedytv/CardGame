using System;
using Godot;
using System.Collections.Generic;

namespace CardGame.Server {

	public class RealMessenger : BaseMessenger
	{
		
		
		public RealMessenger()
		{
			Name = "Messenger";
		}

		public override void OnPlayExecuted(Player player, System.Object Event)
		{
			
		}

		public override void Update(List<Player> players)
		{
			
		}

		public override void DisqualifyPlayer(int player, int reason)
		{
			GD.Print(String.Format("Player {0} disqualified because: {1}", player, reason));
		}

		[Master]
		public override void Deploy(int player, int card)
		{
			EmitSignal(nameof(Deployed), player, card);
		}

		[Master]
		public override void Attack(int player, int attacker, int defender)
		{
			EmitSignal(nameof(Attacked), player, attacker, defender);
		}

		[Master]
		public override void Activate(int player, int card, int skillIndex, List<int> targets)
		{
			EmitSignal(nameof(Activated), player, card, skillIndex, targets);
		}

		[Master]
		public override void SetFaceDown(int player, int faceDown)
		{
			EmitSignal(nameof(FaceDownSet), player, faceDown);
		}

		[Master]
		public override void Target(int player, int target)
		{
			EmitSignal(nameof(Targeted), target);
		}

		[Master]
		public override void PassPlay(int player)
		{
			EmitSignal(nameof(PassedPriority), player);
		}

		[Master]
		public override void EndTurn(int player)
		{
			EmitSignal(nameof(EndedTurn), player);
		}

		[Master]
		public override void SetReady(int player)
		{
			EmitSignal(nameof(PlayerSeated), player);
		}
	}
}
