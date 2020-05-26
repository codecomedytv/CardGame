using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {

	public class Game : Node
	{
		
		private List<Player> Players;
		private Node Messenger = new Messenger();
		private Reference Gamestate = new Gamestate();
		private Reference Battle = new Battle();
		private Reference Link = new Link();
		private Reference Judge = new Judge();
		
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
			connect(Messenger, "Targeted", Gamestate, "OnTargetsSelected");
			
		}

		public void OnPlayerSeated(int ID)
		{
			
		}
		
		public void BeginTurn()
		{
			
		}
		
		public void OnDeployDeclared(int PlayerID, int CardID)
		{
			
		}
		
		public void OnAttackDeclared(int PlayerID, int AttackerID, int DefenderID)
		{
			
		}
		
		public void OnSetFacedownDeclared(int PlayerID, int FacedownID)
		{
			
		}
		
		public void OnActivationDeclared(int PlayerID, int CardID, int SkillIndex, List<int> Targets)
		{
			
		}
		
		public void OnEndTurnDeclared(int PlayerID)
		{
			
		}
		
		private void connect(Godot.Object Emitter, string Signal, Godot.Object Receiver, String Method, Godot.Collections.Array Binds = default(Godot.Collections.Array))
		{
			Godot.Error Error = Emitter.Connect(Signal, Receiver, Method, Binds);
			if(Error != Error.Ok) { GD.PushWarning(Error.ToString()); }
		}
	}
}
