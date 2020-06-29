using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
using Godot;

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
			switch (gameEvent)
			{
				case LoadDeck loadDeck:
				{
					RpcId(loadDeck.Player.Id, "LoadDeck", loadDeck.Deck.ToDictionary(c => c.Id, c => c.SetCode));
					break;
				}
				case Draw draw:
				{
					RpcId(draw.Controller.Id, "QueueDraw", draw.Card.Id, draw.Card.SetCode);
					RpcId(draw.Controller.Opponent.Id, "QueueDraw");
					break;
				}
				case Deploy deploy:
				{
					RpcId(deploy.Controller.Id, "QueueDeploy", deploy.Card.Id);
					RpcId(deploy.Controller.Opponent.Id, "QueueDeploy", deploy.Card.Id, deploy.Card.SetCode);
					break;
				}
			}
		}

		public virtual void Update(IEnumerable<Player> players)
		{
			foreach (var player in players)
			{
				var clientViewableCards = new List<Card>();
				clientViewableCards.AddRange(player.Hand);
				clientViewableCards.AddRange(player.Field);
				clientViewableCards.AddRange(player.Support);
				foreach (var card in clientViewableCards)
				{
					RpcId(player.Id, "SetCardState", card.Id, card.State);
				}
				clientViewableCards.Clear();
				RpcId(player.Id, "QueueState", player.State);
				RpcId(player.Id, "ExecuteEvents");
			}
		}

		public virtual void DisqualifyPlayer(int playerId)
		{
			RpcId(playerId, "Disqualify");
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
