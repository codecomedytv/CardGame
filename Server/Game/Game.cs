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
			connect(Judge, nameof(Judge.Disqualified), this, nameof(OnPlayerDisqualified));
			foreach (var player in Players.Values)
			{
				connect(player, nameof(Player.PlayExecuted), this.Messenger, nameof(Messenger.OnPlayExecuted));
				var bounds = new Godot.Collections.Array { player.Opponent };
				connect(player, nameof(Player.Register), Link, nameof(Link.Register));
				connect(player, nameof(Player.Deployed), Link, nameof(Link.Broadcast));
				connect(player, nameof(Player.Paused), GameState, nameof(GameState.Pause));
				
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
			// TurnPlayer.State = new Idle();
			// TurnPlayer.Opponent.State = new Passive();
			TurnPlayer.SetPlayableCards();
			//TurnPlayer.Legalize();
			foreach (var card in TurnPlayer.Hand)
			{
				TurnPlayer.SetLegal(card);
			}
			TurnPlayer.State = new Idle();
			TurnPlayer.Opponent.State = new Passive();
			TurnPlayer.DeclarePlay(new SetState(TurnPlayer, TurnPlayer.State));
			Update();
		}
		
		public void BeginTurn()
		{
			var player = TurnPlayer;
			player.Draw(1);
			Link.ApplyConstants();
			// player.State = new Idle();
			// player.Opponent.State = new Passive();
			player.SetPlayableCards();
			player.SetAttackers();
			player.SetActivatables();
			foreach (var card in TurnPlayer.Hand)
			{
				TurnPlayer.SetLegal(card);
			}
			player.State = new Idle();
			player.Opponent.State = new Passive();
			player.DeclarePlay(new SetState(player, player.State));
			player.Opponent.DeclarePlay(new SetState(player.Opponent, player.Opponent.State));
			Update();
		}
		
		public void OnDeploy(int playerId, int cardId)
		{
			var player = Players[playerId];
			var card = (Unit)GameState.GetCard(cardId);
			if (Judge.DeployIsIllegalPlay(player, card))
			{
				return;
			}

			player.Deploy(card);
			Link.Register(card);
			Link.ApplyConstants("deploy");
			Link.ApplyTriggered("deploy");
			// player.State = new Acting();
			// player.Opponent.State = new Active();
			player.Opponent.SetActivatables("deploy");
			// player.DeclarePlay(new SetState(player, player.State));
			// player.Opponent.DeclarePlay(new SetState(player.Opponent, player.Opponent.State));
			Link.Broadcast("deploy", new List<Godot.Object>{card});
			player.State = new Acting();
			player.Opponent.State = new Active();
			player.DeclarePlay(new SetState(player, player.State));
			player.Opponent.DeclarePlay(new SetState(player.Opponent, player.Opponent.State));
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
			// player.State = new Acting();
			// player.Opponent.State = new Active();
			// player.DeclarePlay(new SetState(player, player.State));
			// player.Opponent.DeclarePlay(new SetState(player.Opponent, player.Opponent.State));
			Link.SetupManual("attack");
			// Link.Broadcast("attack", [attacker, defender])
			player.State = new Acting();
			player.Opponent.State = new Active();
			player.DeclarePlay(new SetState(player, player.State));
			player.Opponent.DeclarePlay(new SetState(player.Opponent, player.Opponent.State));
			Update();
		}
		
		public void OnSetFaceDown(int playerId, int faceDownId)
		{
			var player = Players[playerId];
			var card = (Support)GameState.GetCard(faceDownId);
			if (Judge.SettingFacedownIsIllegal(player,card))
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
			var player = Players[playerId];
			var card = (Support)GameState.GetCard(cardId);
			GD.Print("Activating ", card.ToString());
			if (Judge.SupportActivationIsIllegal(player, card))
			{
				GD.Print("Support Activation Is Illegal");
				return;
			}
			Link.ApplyConstants();
			player.State = new Acting();
			player.Opponent.State = new Active();
			player.DeclarePlay(new SetState(player, player.State));
			player.Opponent.DeclarePlay(new SetState(player.Opponent, player.Opponent.State));
			Link.Activate(player, card, skillIndex, targets);
			Update();
			
		}

		public void OnPriorityPassed(int playerId)
		{
			var player = Players[playerId];
			player.State = new Passing();
			if(player.Opponent.State.GetType() == typeof(Passing))
			{
				Link.Resolve();
				TurnPlayer.State = new Idle();
				TurnPlayer.Opponent.State = new Passive();
				TurnPlayer.DeclarePlay(new Resolve());
				TurnPlayer.SetValidAttackTargets();
			}
			else
			{
				player.Opponent.State = new Active();
			}
			Update();
			
		}

		public void OnEndTurn(int playerId)
		{
			var player = Players[playerId];
			if (Judge.EndingTurnIsIllegal(player))
			{
				return;
			}
			
			
			player.EndTurn();
			TurnPlayer = player.Opponent;
			GameState.TurnPlayer = TurnPlayer;
			player.IsTurnPlayer = false;
			TurnPlayer.IsTurnPlayer = true;
			foreach (var card in TurnPlayer.Field)
			{
				TurnPlayer.ReadyCard(card);
			}

			foreach (var card in TurnPlayer.Opponent.Support)
			{
				TurnPlayer.Opponent.ReadyCard(card);
			}
			Link.ApplyConstants();
			BeginTurn();
		}

		public void OnPlayerDisqualified(int playerId, int reason)
		{
			// We require this call to be deferred so we can keep the RPC Path Connected until Disconnected
			Players[playerId].Disqualified = true;
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
