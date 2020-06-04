using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CardGame.Server.States;
using Godot.Collections;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

namespace CardGame.Server {

	public class Game : Node
	{
		
		//private List<Player> Players;
		private System.Collections.Generic.Dictionary<int, Player> Players;
		private System.Collections.Generic.Dictionary<int, Card> Cards;
		private readonly BaseMessenger Messenger;
		public readonly Gamestate GameState;
		private readonly Battle Battle = new Battle();
		private readonly Link Link = new Link();
		private readonly Judge Judge = new Judge();
		private Player TurnPlayer;

		[Signal]
		public delegate void GameStateUpdated();
		
		[Signal]
		public delegate void Disqualified();

		public Game() { }

		public Game(List<Player> players, BaseMessenger messenger = null)
		{
			Messenger = messenger ?? new RealMessenger();
			Players = new System.Collections.Generic.Dictionary<int, Player>();
			Cards = new System.Collections.Generic.Dictionary<int, Card>();
			Players[players[0].Id] = players[0];
			Players[players[1].Id] = players[1];
			players[0].Opponent = players[1];
			players[1].Opponent = players[0];
			GameState = new Gamestate();
			Link.SetUp(GameState);
		}

		public override void _Ready()
		{
			AddChild(Messenger);
			connect(Messenger, nameof(BaseMessenger.Targeted), GameState, nameof(Gamestate.OnTargetsSelected));
			connect(Messenger, nameof(BaseMessenger.PlayerSeated), this, nameof(OnPlayerSeated));
			connect(Messenger, nameof(BaseMessenger.EndedTurn), this, nameof(OnEndTurn));
			connect(Messenger, nameof(BaseMessenger.Deployed), this, nameof(OnDeploy));
			connect(Messenger, nameof(BaseMessenger.Attacked), this, nameof(OnAttack));
			connect(Messenger, nameof(BaseMessenger.FaceDownSet),this, nameof(OnSetFaceDown));
			connect(Messenger, nameof(BaseMessenger.Activated), this, nameof(OnActivation));
			connect(Messenger, nameof(BaseMessenger.PassedPriority), this, nameof(OnPriorityPassed));
			foreach (var player in Players.Values)
			{
				connect(player, nameof(Player.Disqualified), this, nameof(OnPlayerDisqualified));
				connect(player, nameof(Player.PlayExecuted), this.Messenger, nameof(Messenger.OnPlayExecuted));
				var bounds = new Godot.Collections.Array { player.Opponent };
				connect(player, nameof(Player.Register), Link, nameof(Link.Register));
				player.Link = Link;
			}

		}

		public void OnPlayerSeated(int id)
		{
			Players[id].Ready = true;
			foreach (var player in Players.Values)
			{
				if (!player.Ready)
				{
					return;
				}
				
			}

			foreach (var player in Players.Values)
			{
				player.LoadDeck(GameState);
				player.Shuffle();
			}

			foreach (var player in Players.Values)
			{
				player.Draw(7);
			}

			TurnPlayer = Players.Values.ToList()[Players.Count - 1];
			TurnPlayer.IsTurnPlayer = true;
			foreach (var card in TurnPlayer.Hand)
			{
				TurnPlayer.SetLegal(card);
			}
			TurnPlayer.SetState(new Idle());
			TurnPlayer.Opponent.SetState(new Passive());
			Update();
		}
		
		public void BeginTurn()
		{
			var player = TurnPlayer;
			player.Draw(1);
			Link.ApplyConstants();
			player.SetAttackers();
			player.SetActivatables();
			foreach (var card in TurnPlayer.Hand)
			{
				TurnPlayer.SetLegal(card);
			}
			player.SetState(new Idle());
			player.Opponent.SetState(new Passive());
			Update();
		}
		
		public void OnDeploy(int playerId, int cardId)
		{
			var player = Players[playerId];
			var card = (Unit)GameState.GetCard(cardId);
			player.OnDeploy(card);
			player.SetState(new Acting());
			player.Opponent.SetState(new Active());
			Update();
			
		}
		
		public void OnAttack(int playerId, int attackerId, int defenderId)
		{
			var player = Players[playerId];
			var attacker = (Unit)GameState.GetCard(attackerId);
			object defender;
			if (defenderId != -1)
			{
				defender = (Unit)GameState.GetCard(defenderId);
			}
			else
			{
				defender = defenderId;
			}

			if (Judge.AttackDeclarationIsIllegal(player, attacker, defender))
			{
				GD.Print("Illegal");
				return;
			}

			GameState.Attacking = attacker;
			if (defenderId != -1)
			{
				player.Opponent.ShowAttack(player, attacker, (Unit)defender);
			}

			Battle.Begin(player, attacker, defender, defenderId == -1);
			Link.AddResolvable(Battle);
			Link.ApplyConstants("attack");
			Link.ApplyTriggered("attack");
			Link.SetupManual("attack");
			player.SetState(new Acting());
			player.Opponent.SetState(new Active());
			Update();
		}
		
		public void OnSetFaceDown(int playerId, int faceDownId)
		{
			var player = Players[playerId];
			var card = (Support)GameState.GetCard(faceDownId);
			player.OnSetFaceDown(card);
			// A New Idle State retriggers the entry state (which gives other cards valid information)
			player.SetState(new Idle());
			Update();
		}
		
		public void OnActivation(int playerId, int cardId, int skillIndex, Array<int> targets)
		{
			var player = Players[playerId];
			var card = (Support)GameState.GetCard(cardId);
			if (Judge.SupportActivationIsIllegal(player, card))
			{
				GD.Print("Support Activation Is Illegal");
				return;
			}

			player.OnActivation(card, skillIndex, targets);
			player.SetState(new Acting());
			player.Opponent.SetState(new Active());
			Update();
			
		}

		public void OnPriorityPassed(int playerId)
		{
			var player = Players[playerId];
			player.SetState(new Passing());
			if(player.Opponent.State.GetType() == typeof(Passing))
			{
				Link.Resolve();
				TurnPlayer.SetState(new Idle());
				TurnPlayer.Opponent.SetState(new Passive());
				TurnPlayer.DeclarePlay(new Resolve());
				TurnPlayer.SetValidAttackTargets();
			}
			else
			{
				player.Opponent.SetState(new Active());
			}
			Update();
			
		}

		public void OnEndTurn(int playerId)
		{
			var player = Players[playerId];
			player.OnEndTurn();
			TurnPlayer = player.Opponent;
			GameState.TurnPlayer = TurnPlayer;
			BeginTurn();
		}

		public void OnPlayerDisqualified(int playerId, int reason)
		{
			// We require this call to be deferred so we can keep the RPC Path Connected until Disconnected
			Players[playerId].IsDisqualified = true;
			Messenger.DisqualifyPlayer(playerId, reason);
			EmitSignal(nameof(Disqualified), playerId);
		}

		public void Update()
		{
			Messenger.Update(Players.Values);
		}
		
		private void connect(Godot.Object emitter, string signal, Godot.Object receiver, string method, Godot.Collections.Array binds = default(Godot.Collections.Array))
		{
			var error = emitter.Connect(signal, receiver, method, binds);
			if(error != Error.Ok) { GD.PushWarning(error.ToString()); }
		}
	}
}
