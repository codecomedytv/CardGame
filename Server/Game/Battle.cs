using Godot;
using System;

namespace CardGame.Server {

	public class Battle : Reference, IResolvable
	{
		private const int DirectAttack = -1;
		private Player Attacking;
		private Player Defending;
		private Card Attacker;
		private object Defender;
		
		public void Begin(Player attacking, Card attacker, object defender)
		{
			Attacking = attacking;
			Defending = attacking.Opponent;
			Attacker = attacker;
			Defender = defender;
		}
		
		public void Resolve(string ignore = "")
		{
			
		}
	}

}
