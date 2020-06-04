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

			if (defenderId is int directAttack && directAttack == -1 && player.Opponent.Field.Count != 0)
			{
				GD.Print(2);
				player.Disqualify();
				return Invalid;
			}
			
			if (defenderId is Card defender && !player.Opponent.Field.Contains(defender))
			{
				GD.Print(3);
				player.Disqualify();
				return Invalid;
			}
			if (defenderId is Card validTarget && !attacker.ValidAttackTargets.Contains(validTarget))
			{
				GD.Print(4);
				player.Disqualify();
				return Invalid;
			}
			return Valid;
		}
		
		public bool SupportActivationIsIllegal(Player player, Support support)
		{

			if (!support.Ready)
			{
				player.Disqualify();
				return Invalid;
			}
			
			else if (!support.CanBeActivated)
			{
				GD.Print("Support Cannot Be Activated");
				player.Disqualify();
				return Invalid;
			}
			
			else if (!player.Support.Contains(support))
			{
				player.Disqualify();
				return Invalid;
			}
			
			else if (support.Activated)
			{	
				player.Disqualify();
				return Invalid;
			}
			return Valid;
		}
		
		}
		
	}

