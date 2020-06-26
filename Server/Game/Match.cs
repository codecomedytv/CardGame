using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Network;
using CardGame.Server.Game.Skills;
using Godot;

namespace CardGame.Server.Game {

	public class Match : Node
	{
		
		// TODO
		// Handle Link Events B
		
		private readonly Messenger Messenger;
		private readonly Players Players;
		private readonly CardCatalog CardCatalog = new CardCatalog();
		public readonly History History = new History();
		public readonly Battle Battle = new Battle();
		public readonly Link Link = new Link();
		public Player TurnPlayer => Players.TurnPlayer;
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
			Link.Players = Players;
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
			ConnectSignals(History, nameof(History.EventRecorded), Link, nameof(Link.OnGameEventRecorded));
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
			
			// Turn Player is (Currently) Choosen When Player Object is created
			TurnPlayer.SetState(States.Idle);
			TurnPlayer.Opponent.SetState(States.Passive);
			Update();
		}
		
		private void BeginTurn()
		{
			TurnPlayer.Draw();
			TurnPlayer.SetState(States.Idle);
			Update();
		}
		
		private void OnDeploy(int playerId, int cardId)
		{
			var player = Players[playerId];
			var card = (Unit)CardCatalog.GetCard(cardId);
			if (!card.CanBeDeployed || player.State != States.Idle)
			{
				Disqualify(player);
				return;
			}
			
			History.Add(new Move(GameEvents.Deploy, player, card, player.Field));
			player.SetState(States.Acting);
			player.Opponent.SetState(States.Active);
			Update();
			
		}

		private void OnAttack(int playerId, int attackerId, int defenderId)
		{
			var player = Players[playerId];
			var attacker = (Unit) CardCatalog.GetCard(attackerId);
			var defender = (Unit) CardCatalog.GetCard(defenderId);
			Attacking = attacker;
			if (!attacker.CanAttack || !player.Opponent.Field.Contains(defender) || !attacker.ValidAttackTargets.Contains(defender) || player.State != States.Idle)
			{
				Disqualify(player);;
				return;
			}
			
			Battle.Begin(player, attacker, defender);
			Link.AddResolvable(Battle);
			History.Add(new DeclareAttack(attacker, defender));
			player.SetState(States.Acting);
			player.Opponent.SetState(States.Active);
			Update();
		}

		private void OnDirectAttack(int playerId, int attackerId)
		{
			var player = Players[playerId];
			var attacker = (Unit) CardCatalog.GetCard(attackerId);
			Attacking = attacker;
			if (!attacker.CanAttack || player.Opponent.Field.Count != 0 || player.State != States.Idle)
			{
				Disqualify(player);
				return;
			}
			
			Battle.BeginDirectAttack(player, attacker);
			Link.AddResolvable(Battle);
			History.Add(new DeclareDirectAttack(attacker));
			player.SetState(States.Acting);
			player.Opponent.SetState(States.Active);
			Update();
		}
		
		private void OnSetFaceDown(int playerId, int faceDownId)
		{
			var player = Players[playerId];
			var card = (Support)CardCatalog.GetCard(faceDownId);
			if (!card.CanBeSet || player.State != States.Idle)
			{
				Disqualify(player);
				return;
			}
			
			History.Add(new Move(GameEvents.SetFaceDown, player, card, player.Support));
			player.SetState(States.Idle);
			Update();
		}
		
		private void OnActivation(int playerId, int cardId, int targetId = 0)
		{
			var player = Players[playerId];
			var card = (Support)CardCatalog.GetCard(cardId);
			var target = CardCatalog.GetCard(targetId);
			var invalidState = !(player.State == States.Idle || player.State == States.Active);
			if (!card.CanBeActivated || invalidState)
			{
				Disqualify(player);
				return;
			}
			
			Link.Activate((Manual) card.Skill, target);
			player.SetState(States.Acting);
			player.Opponent.SetState(States.Active);
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
			if (player.State != States.Active)
			{
				Disqualify(player);
				return;
			}
			if (player.Opponent.State == States.Passing)
			{
				Link.Resolve();
				TurnPlayer.SetState(States.Idle);
				TurnPlayer.Opponent.SetState(States.Passive);
			}
			else
			{
				player.Opponent.SetState(States.Active);
				player.SetState(States.Passing);
			}
			Update();
			
		}


		private void OnEndTurn(int playerId)
		{
			var player = Players[playerId];
			if (player.State != States.Idle)
			{
				Disqualify(player);
				return;
			}
			
			History.Add(new EndTurn(player));
			Players.ChangeTurnPlayer();
			foreach (var unit in player.Opponent.Field) { unit.Ready(); };
			foreach (var support in player.Support) { support.Ready(); }
			TurnPlayer.Draw(); // Does This Trigger Something? Constants?
			TurnPlayer.Opponent.SetState(States.Passive);
			TurnPlayer.SetState(States.Idle);
			Update();
		}

		private void Disqualify(Player player)
		{
			player.IsDisqualified = true;
			Messenger.DisqualifyPlayer(player.Id);
			Messenger.DisqualifyPlayer(player.Opponent.Id);
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
