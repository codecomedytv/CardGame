using Godot;
using System;

namespace CardGame.Server {

	public class Battle : Reference
	{
		private const int DirectAttack = -1;
		private Player Attacking;
		private Player Defending;
		private Card Attacker;
		private Godot.Object Defender;
		
		public void Begin(Player attacking, Card attacker, Godot.Object defender)
		{
			Attacking = attacking;
			Defending = attacking.Opponent;
			Attacker = attacker;
			Defender = defender;
		}
		
		public void Resolve()
		{
			
		}
	}

}