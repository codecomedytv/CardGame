using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Godot.Collections;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

namespace CardGame.Server {

	public class Game : Node
	{
		
		private List<Player> Players;
		private readonly BaseMessenger Messenger;
		public readonly Gamestate GameState;
		private readonly Battle Battle = new Battle();
		private readonly Link Link = new Link();
		private readonly Judge Judge = new Judge();

		[Signal]
		public delegate void GameStateUpdated();
		
		[Signal]
		public delegate void Disqualified();

		public Game() { }

		public Game(List<Player> players, BaseMessenger messenger = null)
		{
			Messenger = messenger ?? new RealMessenger(); 
			Players = players;
			Players[0].Opponent = Players[1];
			Players[1].Opponent = Players[0];
			GameState = new Gamestate(Players);
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
			connect(Messenger, nameof(BaseMessenger.PassedPriority), Link, nameof(Link.OnPriorityPassed));
			connect(Judge, nameof(Judge.Disqualified), this, nameof(OnPlayerDisqualified));
			connect(Link, nameof(Link.Updated), this, nameof(Update));
			foreach (var player in Players)
			{
				connect(player, nameof(Player.PlayExecuted), this.Messenger, nameof(Messenger.OnPlayExecuted));
				connect(player, nameof(Player.TurnEnded), GameState, nameof(GameState.OnTurnEnd));
				var bounds = new Godot.Collections.Array { player.Opponent };
				connect(player, nameof(Player.PriorityPassed), GameState, nameof(GameState.OnPriorityPassed), bounds);
				connect(player, nameof(Player.Register), Link, nameof(Link.Register));
				connect(player, nameof(Player.Deployed), Link, nameof(Link.Broadcast));
				connect(player, nameof(Player.Paused), GameState, nameof(GameState.Pause));
				
			}

		}

		public void OnPlayerSeated(int id)
		{
			GameState.Player(id).Ready = true;
			foreach (var player in Players)
			{
				if (!player.Ready)
				{
					return;
				}
				
			}

			foreach (var player in Players)
			{
				player.LoadDeck(GameState);
				player.Shuffle();
			}

			foreach (var player in Players)
			{
				player.Draw(7);
			}

			var startingPlayer = Players[Players.Count - 1];
			GameState.Begin(startingPlayer);
			startingPlayer.State = Player.States.Idle;
			startingPlayer.Opponent.State = Player.States.Passive;
			startingPlayer.SetPlayableCards();
			startingPlayer.Legalize();
			startingPlayer.DeclareState();
			Update();
		}
		
		public void BeginTurn()
		{
			var player = GameState.GetTurnPlayer();
			player.Draw(1);
			Link.ApplyConstants();
			player.State = Player.States.Idle;
			player.Opponent.State = Player.States.Passive;
			player.SetPlayableCards();
			player.SetAttackers();
			player.SetActivatables();
			player.Legalize();
			player.DeclareState();
			player.Opponent.DeclareState();
			player.ReadyCards();
			Update();
		}
		
		public void OnDeploy(int playerId, int cardId)
		{
			var player = GameState.Player(playerId);
			var card = (Unit)GameState.GetCard(cardId);
			if (Judge.DeployIsIllegalPlay(GameState, player, card))
			{
				return;
			}

			player.Deploy(card);
			Link.Register(card);
			Link.ApplyConstants("deploy");
			Link.ApplyTriggered("deploy");
			player.State = Player.States.Acting;
			player.Opponent.State = Player.States.Active;
			player.Opponent.SetActivatables("deploy");
			player.DeclareState();
			player.Opponent.DeclareState();
			Link.Broadcast("deploy", new List<Godot.Object>{card});
			Update();
			
		}
		
		public void OnAttack(int playerId, int attackerId, int defenderId)
		{
			var player = GameState.Player(playerId);
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

			if (Judge.AttackDeclarationIsIllegal(GameState, player, attacker, defenderId))
			{
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
			player.State = Player.States.Acting;
			player.Opponent.State = Player.States.Active;
			player.DeclareState();
			player.Opponent.DeclareState();
			Link.SetupManual("attack");
			// Link.Broadcast("attack", [attacker, defender])
			Update();
		}
		
		public void OnSetFaceDown(int playerId, int faceDownId)
		{
			var player = GameState.Player(playerId);
			var card = (Support)GameState.GetCard(faceDownId);
			if (Judge.SettingFacedownIsIllegal(GameState, player,card))
			{
				return;
			}
			Link.ApplyConstants();
			player.SetFaceDown(card);
			Link.Register(card);
			Update();
		}
		
		public void OnActivation(int playerId, int cardId, int skillIndex, Array<int> targets)
		{
			var player = GameState.Player(playerId);
			var card = (Support)GameState.GetCard(cardId);
			if (Judge.SupportActivationIsIllegal(GameState, player, card))
			{
				return;
			}
			Link.ApplyConstants();
			player.State = Player.States.Acting;
			player.Opponent.State = Player.States.Active;
			player.DeclareState();
			player.Opponent.DeclareState();
			Link.Activate(player, card, skillIndex, targets);
			Update();
			
		}
		
		public void OnEndTurn(int playerId)
		{
			var player = GameState.Player(playerId);
			if (Judge.EndingTurnIsIllegal(GameState, player))
			{
				return;
			}
			
			// This apparently made no real sense in the GDScript Version?
			player.EmitSignal(nameof(Player.TurnEnded), player.Opponent);
			player.EndTurn();
			Link.ApplyConstants();
			BeginTurn();
		}

		public void OnPlayerDisqualified(int playerId, int reason)
		{
			// We require this call to be deferred so we can keep the RPC Path Connected until Disconnected
			GameState.Player(playerId).Disqualified = true;
			Messenger.DisqualifyPlayer(playerId, reason);
			EmitSignal(nameof(Disqualified), playerId);
		}

		public void Update()
		{
			Messenger.Update(Players);
		}

		private void connect(Godot.Object emitter, string signal, Godot.Object receiver, string method, Godot.Collections.Array binds = default(Godot.Collections.Array))
		{
			var error = emitter.Connect(signal, receiver, method, binds);
			if(error != Error.Ok) { GD.PushWarning(error.ToString()); }
		}
	}
}
