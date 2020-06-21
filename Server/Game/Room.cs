using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Network.Messenger;
using CardGame.Server.States;
using Godot;
using Godot.Collections;

namespace CardGame.Server.Game {

	public class Room : Node
	{
		
		//private List<Player> Players;
		private System.Collections.Generic.Dictionary<int, Player> Players;
		//private System.Collections.Generic.Dictionary<int, Card> Cards;
		private readonly BaseMessenger Messenger;
		public readonly Gamestate GameState;
		private readonly Battle Battle = new Battle();
		private readonly Link Link = new Link();
		private Player TurnPlayer;

		[Signal]
		public delegate void GameStateUpdated();
		
		[Signal]
		public delegate void Disqualified();

		public Room() { }

		public Room(List<Player> players, BaseMessenger messenger = null)
		{
			Messenger = messenger ?? new RealMessenger();
			Players = new System.Collections.Generic.Dictionary<int, Player>();
			//Cards = new System.Collections.Generic.Dictionary<int, Card>();
			Players[players[0].Id] = players[0];
			Players[players[1].Id] = players[1];
			players[0].Opponent = players[1];
			players[1].Opponent = players[0];
			GameState = new Gamestate();
		}

		public override void _Ready()
		{
			AddChild(Messenger);
			connect(Messenger, nameof(BaseMessenger.Targeted), this, nameof(OnTarget));
			connect(Messenger, nameof(BaseMessenger.PlayerSeated), this, nameof(OnPlayerSeated));
			connect(Messenger, nameof(BaseMessenger.EndedTurn), this, nameof(OnEndTurn));
			connect(Messenger, nameof(BaseMessenger.Deployed), this, nameof(OnDeploy));
			connect(Messenger, nameof(BaseMessenger.Attacked), this, nameof(OnAttack));
			connect(Messenger, nameof(BaseMessenger.AttackedDirectly), this, nameof(OnDirectAttack));
			connect(Messenger, nameof(BaseMessenger.FaceDownSet),this, nameof(OnSetFaceDown));
			connect(Messenger, nameof(BaseMessenger.Activated), this, nameof(OnActivation));
			connect(Messenger, nameof(BaseMessenger.PassedPriority), this, nameof(OnPriorityPassed));
			foreach (var player in Players.Values)
			{
				connect(player, nameof(Player.PlayExecuted), this.Messenger, nameof(Messenger.OnPlayExecuted));
				var bounds = new Godot.Collections.Array { player.Opponent };
				player.Link = Link;
				player.Battle = Battle;
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
				player.DrawCards(7);
			}

			TurnPlayer = Players.Values.ToList()[Players.Count - 1];
			TurnPlayer.IsTurnPlayer = true;
			TurnPlayer.SetState(new Idle());
			TurnPlayer.Opponent.SetState(new Passive());
			Update();
		}
		
		public void BeginTurn()
		{
			var player = TurnPlayer;
			// Need to figure a way for this to trigger on state entry?
			// Might be better as command instead
			player.Draw();
			player.SetState(new Idle());
			Link.ApplyConstants();
			Link.SetupManual("");
			Update();
		}
		
		public void OnDeploy(int playerId, int cardId)
		{
			var player = Players[playerId];
			var card = (Unit)GameState.GetCard(cardId);
			var disqualifyPlayer = player.OnDeploy(card);
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);;
			}
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
			
			GameState.Attacking = attacker;
			var disqualifyPlayer = player.OnAttack(attacker, defender, defenderId == -1);
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);;
			}
			Update();
		}

		public void OnDirectAttack(int playerId, int attackerId)
		{
			var player = Players[playerId];
			var attacker = GameState.GetCard(attackerId) as Unit;
			GameState.Attacking = attacker;
			var disqualifyPlayer = player.OnDirectAttack(attacker);
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);
			}
			Update();
		}
		
		
		public void OnSetFaceDown(int playerId, int faceDownId)
		{
			var player = Players[playerId];
			var card = (Support)GameState.GetCard(faceDownId);
			var disqualifyPlayer = player.OnSetFaceDown(card);
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);;
			}
			Update();
		}
		
		public void OnActivation(int playerId, int cardId, int targetId = 0)
		{
			var player = Players[playerId];
			var card = (Support)GameState.GetCard(cardId);
			// Note: We may want to add a null object card for situations like this
			var target = GameState.GetCard(targetId);
			var disqualifyPlayer = player.OnActivation(card, target);
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);;
			}
			Update();
			
		}

		public void OnTarget(int playerId, int targetId)
		{
			// TODO: Refactor Into State
			var player = Players[playerId];
			var target = GameState.GetCard(targetId);
			player.OnTargetSelected(target);
		}

		public void OnPriorityPassed(int playerId)
		{
			var player = Players[playerId];
			var disqualifyPlayer = player.OnPriorityPassed();
			if (disqualifyPlayer)
			{
				Disqualify(player, 0);;
			}
			Update();
			
		}

		public void OnEndTurn(int playerId)
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

		public void Disqualify(Player player, int reason)
		{
			// We require this call to be deferred so we can keep the RPC Path Connected until Disconnected
			player.IsDisqualified = true;
			Messenger.DisqualifyPlayer(player.Id, reason);
			Messenger.DisqualifyPlayer(player.Opponent.Id, 0);
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
