using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {
	
	public interface IMessenger 
	{
		
		void OnPlayExecuted(Player player, System.Object Event);
		void Update(List<Player> Players);
		void DisqualifyPlayer(int ID, int Reason);
		void Deploy(int PlayerID, int CardID);
		void Attack(int PlayerID, int AttackerID, int DefenderID);
		void Activate(int PlayerID, int CardID, int SkillIndex, List<Card> Targets);
		void Target(int PlayerID, int TargetID);
		void PassPlay(int PlayerID);
		void EndTurn(int PlayerID);
		void SetReady(int PlayerID);
		void PlayerSeated(int PlayerID);
		
		
	}
	
}
