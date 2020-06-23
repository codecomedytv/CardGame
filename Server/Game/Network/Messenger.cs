using System.Collections.Generic;
using CardGame.Server.Game.Events;
using Godot;

namespace CardGame.Server.Game.Network {

	public class RealMessenger : BaseMessenger
	{
		
		
		public RealMessenger() => Name = "Messenger";
		
		public override void OnPlayExecuted(Player player, GameEvent gameEvent)
		{
			var message = gameEvent.Message;
			RpcId(player.Id, "QueueEvent", message.Player);
			RpcId(player.Opponent.Id, "QueueEvent", message.Opponent);
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
		public override void DirectAttack(int player, int attacker)
		{
			EmitSignal(nameof(AttackedDirectly));
		}

		[Master]
		public override void Activate(int player, int card, int targetId = 0)
		{
			EmitSignal(nameof(Activated), player, card, targetId);
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
