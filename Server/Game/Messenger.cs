using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
using Godot;
using Card = CardGame.Client.Library.Cards.Card;

namespace CardGame.Server.Game.Network {

	public class Messenger : Node
	{
		[Signal]
		public delegate void Deployed();

		[Signal]
		public delegate void Attacked();

		[Signal]
		public delegate void AttackedDirectly();

		[Signal]
		public delegate void Activated();

		[Signal]
		public delegate void FaceDownSet();

		[Signal]
		public delegate void Targeted();

		[Signal]
		public delegate void EndedTurn();

		[Signal]
		public delegate void PassedPriority();
		
		[Signal]
		public delegate void PlayerSeated(int player);
		
		public Messenger() => Name = "Messenger";
		
		public virtual void OnPlayExecuted(Event gameEvent)
		{
			switch (gameEvent.Identity)
			{
				case GameEvents.Draw:
				{
					var ge = (Move) gameEvent;
					var player = ge.Card.Controller;
					var card = ge.Card;
					RpcId(player.Id, "QueueDraw", card.Id, card.SetCode);
					RpcId(player.Opponent.Id, "QueueDraw");
					break;
				}
				case GameEvents.Deploy:
				{
					var ge = (Move) gameEvent;
					var player = ge.Card.Controller;
					var id = ge.Card.Id;
					RpcId(player.Id, "QueueDeploy", id);
					RpcId(player.Opponent.Id, "QueueDeploy", id, ge.Card.SetCode);
					break;
				}
			}
		}

		public virtual void Update(IEnumerable<Player> players)
		{
			foreach (var player in players) { RpcId(player.Id, "ExecuteEvents");}
		}

		public virtual void DisqualifyPlayer(int player)
		{
			GD.Print($"Player {player} disqualified");
		}

		[Master]
		public void Deploy(int player, int card) => EmitSignal(nameof(Deployed), player, card);
		
		[Master]
		public void Attack(int player, int attacker, int defender)
		{
			EmitSignal(nameof(Attacked), player, attacker, defender);
		}

		[Master]
		public void DirectAttack(int player, int attacker)
		{
			EmitSignal(nameof(AttackedDirectly), player, attacker);
		}

		[Master]
		public void Activate(int player, int card, int targetId = 0)
		{
			EmitSignal(nameof(Activated), player, card, targetId);
		}

		[Master]
		public void SetFaceDown(int player, int faceDown)
		{
			EmitSignal(nameof(FaceDownSet), player, faceDown);
		}

		[Master]
		public void Target(int player, int target) => EmitSignal(nameof(Targeted), player, target);
		
		[Master]
		public void PassPlay(int player) => EmitSignal(nameof(PassedPriority), player);
		
		[Master]
		public void EndTurn(int player) => EmitSignal(nameof(EndedTurn), player);

		[Master]
		public void SetReady(int player) => EmitSignal(nameof(PlayerSeated), player);
	}
}
