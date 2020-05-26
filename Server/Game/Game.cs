using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {

	public class Game : Node
	{
		
		private List<Player> Players;
		private Messenger Messenger = new Messenger();
		private Gamestate Gamestate = new Gamestate();
		private Battle Battle = new Battle();
		private Link Link = new Link();
		private Judge Judge = new Judge();

		[Signal]
		delegate void GamestateUpdated();
		
		[Signal]
		delegate void Disqualified();

		public Game()
		{
		}

		public Game(List<Player> players)
		{
			Players = players;
			Players[0].Opponent = Players[1];
			Players[1].Opponent = Players[0];
		}

		public override void _Ready()
		{
			AddChild(Messenger);
			connect(Messenger, nameof(Messenger.Targeted), Gamestate, nameof(Gamestate.OnTargetsSelected));
			connect(Messenger, nameof(Messenger.PlayerSeated), this, nameof(OnPlayerSeated));
			connect(Messenger, nameof(Messenger.EndedTurn), this, nameof(OnEndTurn));
			connect(Messenger, nameof(Messenger.Deployed), this, nameof(OnDeploy));
			connect(Messenger, nameof(Messenger.Attacked), this, nameof(OnAttack));
			connect(Messenger, nameof(Messenger.FaceDownSet),this, nameof(OnSetFaceDown));
			connect(Messenger, nameof(Messenger.Activated), this, nameof(OnActivation));
			connect(Messenger, nameof(Messenger.PassedPriority), this, nameof(OnPriorityPassed));

		}

		public void OnPlayerSeated(int player)
		{
			
		}
		
		public void BeginTurn()
		{
			
		}
		
		public void OnDeploy(int player, int card)
		{
			
		}
		
		public void OnAttack(int player, int attacker, int defender)
		{
			
		}
		
		public void OnSetFaceDown(int player, int faceDown)
		{
			
		}
		
		public void OnActivation(int player, int card, int skillIndex, List<int> targets)
		{
			
		}

		public void OnPriorityPassed(int player)
		{
			
		}

		public void OnEndTurn(int playerID)
		{
			
		}
		
		private void connect(Godot.Object emitter, string signal, Godot.Object receiver, string method, Godot.Collections.Array binds = default(Godot.Collections.Array))
		{
			Godot.Error Error = emitter.Connect(signal, receiver, method, binds);
			if(Error != Error.Ok) { GD.PushWarning(Error.ToString()); }
		}
	}
}
