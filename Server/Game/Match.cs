using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
using CardGame.Server.Game.Network;
using CardGame.Server.Game.Skills;
using Godot;

namespace CardGame.Server.Game {

	public class Match : Node
	{
		private readonly Messenger Messenger;
		private readonly Players Players;
		private readonly CardCatalog CardCatalog;
		private readonly History History;
		private readonly Link Link;
		public Player TurnPlayer => Players.TurnPlayer;
		
		public Match() { }

		public Match(Players players, Messenger messenger = null)
		{
			Messenger = messenger ?? new Messenger();
			Players = players;
			CardCatalog = new CardCatalog();
			Link = new Link(Players);
			History = new History(Messenger, Link);
		}

		public override void _Ready()
		{
			AddChild(Messenger);
			Messenger.Targeted = OnTarget;
			Messenger.EndedTurn = OnEndTurn;
			Messenger.Deployed = OnDeploy;
			Messenger.Attacked = OnAttack;
			Messenger.AttackedDirectly = OnDirectAttack;
			Messenger.FaceDownSet = OnSetFaceDown;
			Messenger.Activated = OnActivation;
			Messenger.PassedPriority = OnPriorityPassed;
			ConnectSignals(Messenger, nameof(Messenger.PlayerSeated), this, nameof(OnPlayerSeated));
			foreach (var player in Players) { player.History = History; }
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
				player.LoadDeck(CardCatalog);
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
		
		private void OnDeploy(int playerId, int cardId)
		{
			var player = Players[playerId];
			var card = (Unit)CardCatalog[cardId];
			if (card.State != Card.States.CanBeDeployed || player.State != States.Idle)
			{
				Disqualify(player);
				return;
			}

			player.Deploy(card);
			player.SetState(States.Acting);
			player.Opponent.SetState(States.Active);
			Update();
			
		}

		private void OnAttack(int playerId, int attackerId, int defenderId)
		{
			var player = Players[playerId];
			var attacker = (Unit) CardCatalog[attackerId];
			var defender = (Unit) CardCatalog[defenderId];
			if (attacker.State != Card.States.CanAttack || !player.Opponent.Field.Contains(defender) || !attacker.ValidAttackTargets.Contains(defender) || player.State != States.Idle)
			{
				Disqualify(player);;
				return;
			}
			
			attacker.AttackTarget(defender);
			player.SetState(States.Acting);
			player.Opponent.SetState(States.Active);
			Update();
		}

		private void OnDirectAttack(int playerId, int attackerId)
		{
			var player = Players[playerId];
			var attacker = (Unit) CardCatalog[attackerId];
			if (attacker.State != Card.States.CanAttack || player.Opponent.Field.Count != 0 || player.State != States.Idle)
			{
				Disqualify(player);
				return;
			}
			
			attacker.AttackDirectly();
			player.SetState(States.Acting);
			player.Opponent.SetState(States.Active);
			Update();
		}
		
		private void OnSetFaceDown(int playerId, int faceDownId)
		{
			var player = Players[playerId];
			var card = (Support)CardCatalog[faceDownId];
			if (card.State != Card.States.CanBeSet || player.State != States.Idle)
			{
				Disqualify(player);
				return;
			}

			player.SetFaceDown(card);
			player.SetState(States.Idle);
			Update();
		}
		
		private void OnActivation(int playerId, int cardId, int targetId = 0)
		{
			var player = Players[playerId];
			var card = (Support) CardCatalog[cardId];
			var skill = (Manual) card.Skill;
			var target = CardCatalog[targetId];
			var invalidState = !(player.State == States.Idle || player.State == States.Active);
			if (card.State != Card.States.CanBeActivated || invalidState)
			{
				Disqualify(player);
				return;
			}
			skill.Activate(target, Link.NextPositionInLink);
			player.SetState(States.Acting);
			player.Opponent.SetState(States.Active);
			Update();
			
		}

		private void OnTarget(int playerId, int targetId)
		{
			// TODO: Refactor Into State
			var player = Players[playerId];
			var target = CardCatalog[targetId];
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
			TurnPlayer.Draw();
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
			Messenger.Update(CardCatalog, Players);
		}
		
		private static void ConnectSignals(Object emitter, string signal, Object receiver, string method)
		{
			var error = emitter.Connect(signal, receiver, method);
			if(error != Error.Ok) { GD.PushWarning(error.ToString()); }
		}
		
	}
}
