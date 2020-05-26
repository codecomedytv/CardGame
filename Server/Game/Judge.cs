using Godot;
using System;

namespace CardGame.Server {

	public class Judge : Reference
	{
		[Signal]
		delegate void disqualified();
		
		const bool ILLEGAL = true;
		const bool LEGAL = false;
		
		private void Disqualify(Player Disqualified, int Reason)
		{
			
		}
		
		public bool DeployIsIllegalPlay(Gamestate State, Player player, Card Unit)
		{
			return false;
		}
		
		public bool AttackDeclarationIsIllegal(Gamestate State, Player player, Card Attacker, System.Object Defender)
		{
			return false;
		}
		
		public bool SupportActivationIsIllegal(Gamestate State, Player player, Card Support)
		{
			return false;
		}
		
		public bool SettingFacedownIsIllegal(Gamestate State, Player player, Card Support)
		{
			return false;
		}
		
		public bool EndingTurnIsIllegal(Gamestate State, Player player)
		{
			return false;
		}
	}
}
