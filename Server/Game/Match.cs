using System;
using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
using CardGame.Server.Game.Skills;
using Godot;
using Object = Godot.Object;

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
			Messenger.Deploy = Deploy;
			Messenger.SetFaceDown = SetFaceDown;
			Messenger.Attack = Attack;
			Messenger.AttackDirectly = DirectAttack;
			Messenger.Activate = Activate;
			Messenger.Target = Target;
			Messenger.PassPlay = PassPlay;
			Messenger.EndTurn = EndTurn;
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
			TurnPlayer.State = States.Idle;
			TurnPlayer.Opponent.State = States.Passive;
			Update();
		}
		
		private void Deploy(int playerId, int cardId)
		{
			var player = Players[playerId];
			var card = (Unit)CardCatalog[cardId];
			if (card.State != Card.States.CanBeDeployed || player.State != States.Idle)
			{
				Disqualify(player);
				return;
			}

			player.Deploy(card);
			player.State = States.Acting;
			player.Opponent.State = States.Active;
			Update();
			
		}

		private void Attack(int playerId, int attackerId, int defenderId)
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
			player.State = States.Acting;
			player.Opponent.State = States.Active;
			Update();
		}

		private void DirectAttack(int playerId, int attackerId)
		{
			var player = Players[playerId];
			var attacker = (Unit) CardCatalog[attackerId];
			if (attacker.State != Card.States.CanAttackDirectly || player.Opponent.Field.Count != 0 || player.State != States.Idle)
			{
				Disqualify(player);
				return;
			}
			
			attacker.AttackDirectly();
			player.State = States.Acting;
			player.Opponent.State = States.Active;
			Update();
		}
		
		private void SetFaceDown(int playerId, int faceDownId)
		{
			var player = Players[playerId];
			var card = (Support)CardCatalog[faceDownId];
			if (card.State != Card.States.CanBeSet || player.State != States.Idle)
			{
				Disqualify(player);
				return;
			}

			player.SetFaceDown(card);
			player.State = States.Idle;
			Update();
		}
		
		private void Activate(int playerId, int cardId, int targetId = 0)
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
			player.State = States.Acting;
			player.Opponent.State = States.Active;
			Update();
			
		}

		private void Target(int playerId, int targetId)
		{
			// TODO: Refactor Into State
			GD.Print($"{playerId} is playerId");
			GD.Print($"{targetId} is targetId");
			var player = Players[playerId];
			var targetingSkill = player.TargetingSkill;
			var target = CardCatalog[targetId];
			if (player.State != States.Targeting || !targetingSkill.ValidTargets.Contains(target))
			{
				GD.Print("Disqualified");
				Disqualify(player);
				return;
			} 
			GD.Print("Attempting To Resolve");
			player.OnTargetSelected(target);
			targetingSkill.Target = target;
			targetingSkill.Resume();
			Link.Resolve();
			TurnPlayer.State = States.Idle;
			TurnPlayer.Opponent.State = States.Passive;
			Update();
		}

		private void PassPlay(int playerId)
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
				if (TurnPlayer.State != States.Targeting && TurnPlayer.Opponent.State != States.Targeting)
				{
					// We want to skip this if we're currently trying to target something
					TurnPlayer.State = States.Idle;
					TurnPlayer.Opponent.State = States.Passive;
				}
			}
			else
			{
				player.Opponent.State = States.Active;
				player.State = States.Passing;
			}
			Update();
			
		}


		private void EndTurn(int playerId)
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
			TurnPlayer.Opponent.State = States.Passive;
			TurnPlayer.State = States.Idle;
			Update();
		}

		private void Disqualify(Player player)
		{
			player.IsDisqualified = true;
			Messenger.DisqualifyPlayer(player.Id);
			Messenger.DisqualifyPlayer(player.Opponent.Id);
		}
		
		private void Update()
		{
			foreach (var card in CardCatalog)
			{
				card.SetState();
			}
			Messenger.Update(CardCatalog, Players);
		}
		
		private static void ConnectSignals(Object emitter, string signal, Object receiver, string method)
		{
			var error = emitter.Connect(signal, receiver, method);
			if(error != Error.Ok) { GD.PushWarning(error.ToString()); }
		}
		
	}
}
