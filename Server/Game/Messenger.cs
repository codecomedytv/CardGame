using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
using Godot;

namespace CardGame.Server.Game.Network {
	
	public delegate object Message(int id, string method, params object[] arguments);

	public class Messenger : Node
	{
		public Action<int, int> Deployed;
		public Action<int, int, int> Attacked;
		public Action<int, int> AttackedDirectly;
		public Action<int, int, int> Activated;
		public Action<int, int> FaceDownSet;
		public Action<int, int> Targeted;
		public Action<int> PassedPriority;
		public Action<int> EndedTurn;
		
		
		[Signal]
		public delegate void PlayerSeated(int player);
		
		private readonly Message Message; 

		public Messenger()
		{
			Name = "Messenger";
			Message = RpcId;
		}
		public virtual void OnPlayExecuted(Event gameEvent) => gameEvent.SendMessage(Message);

		public virtual void Update(IEnumerable<Card> cards, IEnumerable<Player> players)
		{
			foreach (var card in cards)
			{
				card.Update(Message);
			}
			foreach (var player in players)
			{
				RpcId(player.Id, "ExecuteEvents", player.State);
			}
		}

		public virtual void DisqualifyPlayer(int playerId)
		{
			RpcId(playerId, "Disqualify");
		}

		[Master]
		public void Deploy(int player, int card) => Deployed(player, card);
		
		[Master]
		public void Attack(int player, int attacker, int defender)
		{
			Attacked(player, attacker, defender);
		}

		[Master]
		public void DirectAttack(int player, int attacker)
		{
			AttackedDirectly(player, attacker);
		}

		[Master]
		public void Activate(int player, int card, int targetId = 0)
		{
			Activated(player, card, targetId);
		}

		[Master]
		public void SetFaceDown(int player, int faceDown)
		{
			FaceDownSet(player, faceDown);
		}

		[Master]
		public void Target(int player, int target) => Targeted(player, target);
		
		[Master]
		public void PassPlay(int player) => PassedPriority(player);
		
		[Master]
		public void EndTurn(int player) => EndedTurn(player);

		[Master]
		public void SetReady(int player) => EmitSignal(nameof(PlayerSeated), player);
	}
}
