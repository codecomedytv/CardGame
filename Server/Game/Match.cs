using System.Collections.Generic;
using System.ComponentModel.Design;
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
		public Player TurnPlayer;
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
			ConnectSignals(History, nameof(History.EventRecorded), Link, nameof(Link.Broadcast));
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
			
			// TODO: Access TurnPlayer Directly from Tests to avoid Index Misalignment
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
			Link.SetupManual(new NullCommand());
			Update();
		}
		
		private void OnDeploy(int playerId, int cardId)
		{
			var player = Players[playerId];
			var card = (Unit)CardCatalog.GetCard(cardId);
			if (!card.CanBeDeployed || player.State.ToString() != "Idle")
			{
				Disqualify(player, 0);
				Update();
				return;
			}
			
			Link.Register(card);
			History.Add(new Move(GameEvents.Deploy, player, card, player.Field));
			player.SetState(new Acting());
			player.Opponent.SetState(new Active());
			Update();
			
		}

		private void OnAttack(int playerId, int attackerId, int defenderId)
		{
			var player = Players[playerId];
			var attacker = (Unit)CardCatalog.GetCard(attackerId);
			var defender = (Unit) CardCatalog.GetCard(defenderId);
			Attacking = attacker;
			if (!attacker.CanAttack || !player.Opponent.Field.Contains(defender) || !attacker.ValidAttackTargets.Contains(defender) || player.State.ToString() != "Idle")
			{
				Disqualify(player, 0);;
				Update();
				return;
			}
			
			Battle.Begin(player, attacker, defender);
			Link.AddResolvable(Battle);
			History.Add(new DeclareAttack(attacker, defender));
			player.SetState(new Acting());
			player.Opponent.SetState(new Active());
			Update();
		}

		private void OnDirectAttack(int playerId, int attackerId)
		{
			var player = Players[playerId];
			var attacker = (Unit) CardCatalog.GetCard(attackerId);
			Attacking = attacker;
			if (!attacker.CanAttack || player.Opponent.Field.Count != 0 || player.State.ToString() != "Idle")
			{
				Disqualify(player, 0);
				Update();
				return;
			}
			
			Battle.BeginDirectAttack(player, attacker);
			Link.AddResolvable(Battle);
			History.Add(new DeclareDirectAttack(attacker));
			player.SetState(new Acting());
			player.Opponent.SetState(new Active());
			Update();
		}
		
		private void OnSetFaceDown(int playerId, int faceDownId)
		{
			var player = Players[playerId];
			var card = (Support)CardCatalog.GetCard(faceDownId);
			if (!card.CanBeSet || player.State.ToString() != "Idle")
			{
				Disqualify(player, 0);;
				Update();
				return;
			}
			
			Link.ApplyConstants();
			Link.Register(card);
			History.Add(new Move(GameEvents.SetFaceDown, player, card, player.Support));
			player.SetState(new Idle());
			Update();
		}
		
		private void OnActivation(int playerId, int cardId, int targetId = 0)
		{
			var player = Players[playerId];
			var card = (Support)CardCatalog.GetCard(cardId);
			var target = CardCatalog.GetCard(targetId);
			var invalidState = !(player.State.ToString() == "Idle" || player.State.ToString() == "Active");
			if (!card.CanBeActivated || invalidState)
			{
				Disqualify(player, 0);
				Update();
				return;
			}
			
			Link.Activate(card.Skill, target);
			player.SetState(new Acting());
			player.Opponent.SetState(new Active());
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
			if (player.State.ToString() != "Active")
			{
				Disqualify(player, 0);
				Update();
				return;
			}
			if (player.Opponent.State.GetType() == typeof(Passing))
			{
				Link.Resolve();
				var turnPlayer = player.IsTurnPlayer ? player : player.Opponent;
				turnPlayer.SetState(new Idle());
				turnPlayer.Opponent.SetState(new Passive());
			}
			else
			{
				player.Opponent.SetState(new Active());
				player.SetState(new Passing());
			}
			Update();
			
		}


		private void OnEndTurn(int playerId)
		{
			var player = Players[playerId];
			if (player.State.ToString() != "Idle")
			{
				Disqualify(player, 0);
				Update();
				return;
			}
			
			player.Match.History.Add(new EndTurn(player));
			player.IsTurnPlayer = false;
			player.Opponent.IsTurnPlayer = true;
			Link.ApplyConstants();
			foreach (var unit in player.Opponent.Field) { unit.Ready(); };
			foreach (var support in player.Support) { support.Ready(); }
			player.SetState(new Passive());
			player.Opponent.SetState(new Idle());
			TurnPlayer = player.Opponent;
			BeginTurn();
		}

		private void Disqualify(Player player, int reason)
		{
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

	internal class NullCommand : Command
	{
		public override void Execute()
		{
			
		}

		public override void Undo()
		{
			
		}
	}
}
