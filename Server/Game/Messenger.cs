using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {

	public class Messenger : Node, IMessenger
	{
			
		public void OnPlayExecuted(Player player, System.Object Event) {}
		public void Update(List<Player> Players) {}
		public void DisqualifyPlayer(int ID, int Reason) {}
		public void Deploy(int PlayerID, int CardID) {}
		public void Attack(int PlayerID, int AttackerID, int DefenderID) {}
		public void Activate(int PlayerID, int CardID, int SkillIndex, List<Card> Targets) {}
		public void Target(int PlayerID, int TargetID) {}
		public void PassPlay(int PlayerID) {}
		public void EndTurn(int PlayerID) {}
		public void SetReady(int PlayerID) {}
		public void PlayerSeated(int PlayerID) {}
	}
}
