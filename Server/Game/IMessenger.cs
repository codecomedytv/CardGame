using Godot;
using System;
using System.Collections.Generic;

namespace CardGame.Server {
	
	public interface IMessenger 
	{
		
		void OnPlayExecuted(Player player, System.Object gameEvent);
		void Update(List<Player> players);
		void DisqualifyPlayer(int player, int reason);
		void Deploy(int player, int card);
		void Attack(int player, int attacker, int defender);
		void Activate(int player, int card, int skillIndex, List<int> targets);
		void SetFaceDown(int player, int card);
		void Target(int player, int target);
		void PassPlay(int player);
		void EndTurn(int player);
		void SetReady(int player);
		
		
	}
	
}
