using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Network;
using CardGame.Server.States;
using Godot;

namespace CardGame.Server.Game {

	public class Match : Node
	{
		
		private readonly Messenger Messenger;
		private readonly Players Players;
		private readonly CardCatalog CardCatalog = new CardCatalog();
		public readonly History History = new History();
		public readonly Battle Battle = new Battle();
		public readonly Link Link = new Link();
		private Player TurnPlayer;
		public Unit Attacking;

		[Signal]
		public delegate void GameStateUpdated();
		
		[Signal]
		public delegate void Disqualified();

		public Match() { }

		public Match(Players players, Messenger messenger = null)
		{
			Messenger = messenger ?? new Messenger();
			Players = players;

		}

		public override void _Ready()
		{
			AddChild(Messenger);
			ConnectSignals(Messenger, nameof(Messenger.Targeted), this, nameof(OnTarget));
			ConnectSignals(Messenger, nameof(Messenger.PlayerSeated), this, nameof(OnPlayerSeated));
			ConnectSignals(Messenger, nameof(Messenger.EndedTurn), this, nameof(OnEndTurn));
			ConnectSignals(Messenger, nameof(Messenger.Deployed), this, nameof(OnDeploy));
			ConnectSignals(Messenger, nameof(Messenger.Attacked), this, nameof(OnAttack));
			ConnectSignals(Messenger, nameof(Messenger.AttackedDirectly), this, nameof(OnDirectAttack));
			ConnectSignals(Messenger, nameof(Messenger.FaceDownSet),this, nameof(OnSetFaceDown));
			ConnectSignals(Messenger, nameof(Messenger.Activated), this, nameof(OnActivation));
			ConnectSignals(Messenger, nameof(Messenger.PassedPriority), this, nameof(OnPriorityPassed));
			ConnectSignals(History, nameof(History.EventRecorded), Messenger, nameof(Messenger.OnPlayExecuted));
			foreach (var player in Players) { player.Match = this; }

		}

		public void OnPlayerSeated(int id)
		{
			Players[id].Ready = true;
			if (Players.Any(player => !player.Ready))
			{
				return;
			}

			foreach (var player in Players)
			{
				player.Match.History.Add(new LoadDeck(player, this));
				player.Shuffle();
				for (var i = 0; i < 7; i++)
				{
					player.Draw();
				}
			}
			
			TurnPlayer = Players.TurnPlayer();
			TurnPlayer.IsTurnPlayer = true;
			TurnPlayer.SetState(new Idle());
			TurnPlayer.Opponent.SetState(new Passive());
			Update();
		}
		
		private void BeginTurn()
		{
			var player = TurnPlayer;
			player.Draw();
			player.SetState(new Idle());
			Link.ApplyConstants();
			Link.SetupManual("");
			Update();
		}
		
		private void OnDeploy(int playerId, int cardId)
		{
			var player = Players[playerId];
			var card = (Unit)CardCatalog.GetCard(cardId);
			var disqualifyPlayer = player.OnDeploy(card);
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);;
			}
			Update();
			
		}
		
		private void OnAttack(int playerId, int attackerId, int defenderId)
		{
			var player = Players[playerId];
			var attacker = CardCatalog.GetCard(attackerId) as Unit;
			var defender = CardCatalog.GetCard(defenderId) as Unit;
			Attacking = attacker;
			var disqualifyPlayer = player.OnAttack(attacker, defender);
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);;
			}
			Update();
		}

		private void OnDirectAttack(int playerId, int attackerId)
		{
			var player = Players[playerId];
			var attacker = CardCatalog.GetCard(attackerId) as Unit;
			Attacking = attacker;
			var disqualifyPlayer = player.OnDirectAttack(attacker);
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);
			}
			Update();
		}
		
		
		private void OnSetFaceDown(int playerId, int faceDownId)
		{
			var player = Players[playerId];
			var card = (Support)CardCatalog.GetCard(faceDownId);
			var disqualifyPlayer = player.OnSetFaceDown(card);
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);;
			}
			Update();
		}
		
		private void OnActivation(int playerId, int cardId, int targetId = 0)
		{
			var player = Players[playerId];
			var card = (Support)CardCatalog.GetCard(cardId);
			// Note: We may want to add a null object card for situations like this
			var target = CardCatalog.GetCard(targetId);
			var disqualifyPlayer = player.OnActivation(card, target);
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);;
			}
			Update();
			
		}

		private void OnTarget(int playerId, int targetId)
		{
			// TODO: Refactor Into State
			var player = Players[playerId];
			var target = CardCatalog.GetCard(targetId);
			player.OnTargetSelected(target);
		}

		private void OnPriorityPassed(int playerId)
		{
			var player = Players[playerId];
			var disqualifyPlayer = player.OnPriorityPassed();
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);;
			}
			Update();
			
		}

		private void OnEndTurn(int playerId)
		{
			var player = Players[playerId];
			var disqualifyPlayer = player.OnEndTurn();
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);;
			}
			TurnPlayer = player.Opponent;
			BeginTurn();
		}

		private void Disqualify(Player player, int reason)
		{
			// We require this call to be deferred so we can keep the RPC Path Connected until Disconnected
			player.IsDisqualified = true;
			Messenger.DisqualifyPlayer(player.Id, reason);
			Messenger.DisqualifyPlayer(player.Opponent.Id, 0);
		}
		
		public void RegisterCard(Card card) => CardCatalog.RegisterCard(card);

		private void Update()
		{
			Messenger.Update(Players);
		}
		
		private static void ConnectSignals(Object emitter, string signal, Object receiver, string method)
		{
			var error = emitter.Connect(signal, receiver, method);
			if(error != Error.Ok) { GD.PushWarning(error.ToString()); }
		}
	}
}
