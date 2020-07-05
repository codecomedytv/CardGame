using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
using Godot;

namespace CardGame.Server.Game {
	
	public delegate object Message(int id, string method, params object[] arguments);

	public class Messenger : Node
	{
		public Action<int, int> Deploy;
		public Action<int, int, int> Attack;
		public Action<int, int> AttackDirectly;
		public Action<int, int, int> Activate;
		public Action<int, int> SetFaceDown;
		public Action<int, int> Target;
		public Action<int> PassPlay;
		
		[Master]
		public Action<int> EndTurn;
		
		
		[Signal]
		public delegate void PlayerSeated(int player);
		
		private readonly Message Message; 

		public Messenger()
		{
			Name = "Messenger";
			Message = SendMessage;
		}

		private object SendMessage(int id, string signal, params object[] args)
		{
			return RpcId(id, "Queue", signal, args.ToList());
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
				RpcId(player.Id, "Execute", player.State);
			}
		}

		public virtual void DisqualifyPlayer(int playerId)
		{
			RpcId(playerId, "Disqualify");
		}

		[Master]
		public void OnDeployDeclared(int player, int card) => Deploy(player, card);
		
		[Master]
		public void OnAttackDeclared(int player, int attacker, int defender)
		{
			Attack(player, attacker, defender);
		}

		[Master]
		public void OnDirectAttackDeclared(int player, int attacker)
		{
			AttackDirectly(player, attacker);
		}

		[Master]
		public void OnActivationDeclared(int player, int card, int targetId = 0)
		{
			Activate(player, card, targetId);
		}

		[Master]
		public void OnSetFaceDownDeclared(int player, int faceDown)
		{
			SetFaceDown(player, faceDown);
		}

		[Master]
		public void OnTargetDeclared(int player, int target) => Target(player, target);
		
		[Master]
		public void OnPassPlayDeclared(int player) => PassPlay(player);
		
		[Master]
		public void OnEndTurnDeclared(int player) => EndTurn(player);

		[Master]
		public void SetReady(int player) => EmitSignal(nameof(PlayerSeated), player);
	}
}
