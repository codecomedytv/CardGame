using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {

	public class Game : Node
	{
		
		private List<Player> Players;
		private Node Messenger;
		private Reference Gamestate;
		private Reference Battle;
		private Reference Link;
		private Reference Judge;
		
		[Signal]
		delegate void GamestateUpdated();
		
		[Signal]
		delegate void Disqualified();
		
		public Game() {}
		
		public Game(List<Player> players)
		{
			Players = players;
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
		
		private void connect(Godot.Object Emitter, string Signal, Godot.Object Receiver, String Method, Godot.Collections.Array Binds)
		{
			Godot.Error Error = Emitter.Connect(Signal, Receiver, Method, Binds);
			if(Error != Error.Ok) { GD.PushWarning(Error.ToString()); }
		}
	}
}
