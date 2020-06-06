using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

namespace CardGame.Server {

	public class RealMessenger : BaseMessenger
	{
		
		
		public RealMessenger() => Name = "Messenger";
		
		public override void OnPlayExecuted(Player player, GameEvent gameEvent)
		{
			var message = gameEvent.GetMessage();
			RpcId(player.Id, "QueueEvent", message.Player["command"], message.Player["args"]);
			RpcId(player.Opponent.Id, "QueueEvent", message.Opponent["command"], message.Opponent["args"]);
		}

		public override void Update(IEnumerable<Player> players)
		{
			foreach (var player in players) { RpcId(player.Id, "ExecuteEvents");}
		}

		public override void DisqualifyPlayer(int player, int reason)
		{
			GD.Print($"Player {player} disqualified because: {reason}");
		}

		[Master]
		public override void Deploy(int player, int card) => EmitSignal(nameof(Deployed), player, card);
		
		[Master]
		public override void Attack(int player, int attacker, int defender)
		{
			EmitSignal(nameof(Attacked), player, attacker, defender);
		}

		[Master]
		public override void Activate(int player, int card, Array<int> targets)
		{
			EmitSignal(nameof(Activated), player, card, targets);
		}

		[Master]
		public override void SetFaceDown(int player, int faceDown)
		{
			EmitSignal(nameof(FaceDownSet), player, faceDown);
		}

		[Master]
		public override void Target(int player, int target) => EmitSignal(nameof(Targeted), player, target);
		
		[Master]
		public override void PassPlay(int player) => EmitSignal(nameof(PassedPriority), player);
		
		[Master]
		public override void EndTurn(int player) => EmitSignal(nameof(EndedTurn), player);

		[Master]
		public override void SetReady(int player) => EmitSignal(nameof(PlayerSeated), player);
	}
}
