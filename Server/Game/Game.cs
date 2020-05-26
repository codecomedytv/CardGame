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
			connect(Messenger, nameof(Messenger.FaceDownSet),this, nameof(OnSetFacedown));
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
		
		public void OnSetFacedown(int player, int faceDown)
		{
			
		}
		
		public void OnActivation(int player, int card, int skillIndex, List<int> targets)
		{
			
		}

		public void OnPriorityPassed(int player)
		{
			
		}

		public void OnEndTurn(int PlayerID)
		{
			
		}
		
		private void connect(Godot.Object Emitter, string Signal, Godot.Object Receiver, String Method, Godot.Collections.Array Binds = default(Godot.Collections.Array))
		{
			Godot.Error Error = Emitter.Connect(Signal, Receiver, Method, Binds);
			if(Error != Error.Ok) { GD.PushWarning(Error.ToString()); }
		}
	}
}
