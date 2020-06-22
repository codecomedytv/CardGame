using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Room.Cards;
using CardGame.Server.Room.Network.Messenger;
using CardGame.Server.States;
using Godot;


namespace CardGame.Server.Room {

	public class Game : Node
	{
		
		private readonly BaseMessenger Messenger;
		private Dictionary<int, Player> Players;
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

		public Game() { }

		public Game(List<Player> players, BaseMessenger messenger = null)
		{
			Messenger = messenger ?? new RealMessenger();
			Players = new System.Collections.Generic.Dictionary<int, Player>();
			Players[players[0].Id] = players[0];
			Players[players[1].Id] = players[1];
			players[0].Opponent = players[1];
			players[1].Opponent = players[0];
		}

		public override void _Ready()
		{
			AddChild(Messenger);
			ConnectSignals(Messenger, nameof(BaseMessenger.Targeted), this, nameof(OnTarget));
			ConnectSignals(Messenger, nameof(BaseMessenger.PlayerSeated), this, nameof(OnPlayerSeated));
			ConnectSignals(Messenger, nameof(BaseMessenger.EndedTurn), this, nameof(OnEndTurn));
			ConnectSignals(Messenger, nameof(BaseMessenger.Deployed), this, nameof(OnDeploy));
			ConnectSignals(Messenger, nameof(BaseMessenger.Attacked), this, nameof(OnAttack));
			ConnectSignals(Messenger, nameof(BaseMessenger.AttackedDirectly), this, nameof(OnDirectAttack));
			ConnectSignals(Messenger, nameof(BaseMessenger.FaceDownSet),this, nameof(OnSetFaceDown));
			ConnectSignals(Messenger, nameof(BaseMessenger.Activated), this, nameof(OnActivation));
			ConnectSignals(Messenger, nameof(BaseMessenger.PassedPriority), this, nameof(OnPriorityPassed));
			foreach (var player in Players.Values)
			{
				ConnectSignals(player, nameof(Player.PlayExecuted), History, nameof(History.OnPlayExecuted));
				ConnectSignals(player, nameof(Player.PlayExecuted), Messenger, nameof(Messenger.OnPlayExecuted));
				player.Game = this;
			}

		}

		public void OnPlayerSeated(int id)
		{
			Players[id].Ready = true;
			if (Players.Values.Any(player => !player.Ready))
			{
				return;
			}

			foreach (var player in Players.Values)
			{
				player.LoadDeck(this);
				player.Shuffle();
			}

			foreach (var player in Players.Values)
			{
				{
					for (var i = 0; i < 7; i++)
					{
						player.Draw();
					}
				}
			}

			TurnPlayer = Players.Values.ToList()[Players.Count - 1];
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
			Messenger.Update(Players.Values);
		}
		
		private static void ConnectSignals(Object emitter, string signal, Object receiver, string method)
		{
			var error = emitter.Connect(signal, receiver, method);
			if(error != Error.Ok) { GD.PushWarning(error.ToString()); }
		}
	}
}
