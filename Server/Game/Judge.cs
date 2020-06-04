using Godot;
using System;
using System.Linq;
using CardGame.Server.States;

namespace CardGame.Server {

	public class Judge : Reference
	{
		private const bool Invalid = true;
		private const bool Valid = false;

		public bool DeployIsIllegalPlay(Player player, Unit unit)
		{
			// if(player.State.GetType() != typeof(Idle))
			// {
			// 	player.Disqualify();
			// 	return Invalid;
			// }
			
			if (player.Field.Count == 7)
			{
				player.Disqualify();
				return Invalid;
			}
			
			else if (!player.Hand.Contains(unit))
			{
				player.Disqualify();
				return Invalid;
			}
			
			else if (!unit.CanBeDeployed)
			{
				player.Disqualify();
				return Invalid;
			}
			return Valid;
		}
		
		public bool AttackDeclarationIsIllegal(Player player, Unit attacker, object defenderId)
		{
			// if (player.State.GetType() != typeof(Idle))
			// {
			// 	GD.Print(0);
			// 	player.Disqualify();
			// 	return Invalid;
			// }
			
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
			//if (!player.Active())
			// if (!(player.State.GetType() == typeof(Idle) || player.State.GetType() == typeof(Active)))
			// {
			// 	player.Disqualify();
			// 	return Invalid;
			// }

			if (!support.Ready)
			{
				player.Disqualify();
				return Invalid;
			}
			
			else if (!support.CanBeActivated)
			{
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
		
		public bool SettingFacedownIsIllegal(Player player, Support support)
		{
			// if (player.State.GetType() != typeof(Idle))
			// {
			// 	player.Disqualify();
			// 	return Invalid;
			// }
			
			if (!player.Hand.Contains(support))
			{
				player.Disqualify();
				return Invalid;
			}
			
			else if (!support.CanBeSet)
			{
				player.Disqualify();
				return Invalid;
			}
			return Valid;
		}
		
		public bool EndingTurnIsIllegal(Player player)
		{
			if (player.State.GetType() != typeof(Idle))
			{
				player.Disqualify();
				return Invalid;
			}
			return Valid;
		}
	}
}
