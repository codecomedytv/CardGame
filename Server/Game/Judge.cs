using Godot;
using System;
using System.Linq;
using CardGame.Server.States;

namespace CardGame.Server {

	public class Judge : Reference
	{
		private const bool Invalid = true;
		private const bool Valid = false;
		
		public bool AttackDeclarationIsIllegal(Player player, Unit attacker, object defenderId)
		{
			if (!attacker.Ready)
			{
				GD.Print(1);
				player.Disqualify();
				return Invalid;
			}

			switch (defenderId)
			{
				case int directAttack when directAttack == -1 && player.Opponent.Field.Count != 0:
					GD.Print(2);
					player.Disqualify();
					return Invalid;
				case Card defender when !player.Opponent.Field.Contains(defender):
					GD.Print(3);
					player.Disqualify();
					return Invalid;
				case Card validTarget when !attacker.ValidAttackTargets.Contains(validTarget):
					GD.Print(4);
					player.Disqualify();
					return Invalid;
				default:
					return Valid;
			}
		}
		
	}
}

