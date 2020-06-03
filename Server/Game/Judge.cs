using Godot;
using System;
using System.Linq;
using CardGame.Server.States;

namespace CardGame.Server {

	public class Judge : Reference
	{
		[Signal]
		public delegate void Disqualified();

		enum Reasons
		{
			OpponentDisqualified,
			InvalidDeploy,
			InvalidAttack,
			InvalidActivation,
			InvalidSet,
			InvalidEndTurn
		};
		private const bool Invalid = true;
		private const bool Valid = false;
		
		private void Disqualify(Player player, Reasons reason)
		{
			EmitSignal(nameof(Disqualified), player.Id, (int)reason);
			EmitSignal(nameof(Disqualified), player.Opponent.Id, (int)Reasons.OpponentDisqualified);
		}
		
		public bool DeployIsIllegalPlay(Player player, Unit unit)
		{
			if(player.State.GetType() != typeof(Idle))
			{
				Disqualify(player, Reasons.InvalidDeploy);
				return Invalid;
			}
			
			else if (player.Field.Count == 7)
			{
				Disqualify(player, Reasons.InvalidDeploy);
				return Invalid;
			}
			
			else if (!player.Hand.Contains(unit))
			{
				Disqualify(player, Reasons.InvalidDeploy);
				return Invalid;
			}
			
			else if (!unit.CanBeDeployed)
			{
				Disqualify(player, Reasons.InvalidDeploy);
				return Invalid;
			}
			return Valid;
		}
		
		public bool AttackDeclarationIsIllegal(Player player, Unit attacker, object defenderId)
		{
			if (player.State.GetType() != typeof(Idle))
			{
				GD.Print(0);
				Disqualify(player, Reasons.InvalidAttack);
				return Invalid;
			}
			
			if (!attacker.Ready)
			{
				GD.Print(1);
				Disqualify(player, Reasons.InvalidAttack);
				return Invalid;
			}

			if (defenderId is int directAttack && directAttack == -1 && player.Opponent.Field.Count != 0)
			{
				GD.Print(2);
				Disqualify(player, Reasons.InvalidAttack);
				return Invalid;
			}
			
			if (defenderId is Card defender && !player.Opponent.Field.Contains(defender))
			{
				GD.Print(3);
				Disqualify(player, Reasons.InvalidAttack);
				return Invalid;
			}
			if (defenderId is Card validTarget && !attacker.ValidAttackTargets.Contains(validTarget))
			{
				GD.Print(4);
				Disqualify(player, Reasons.InvalidAttack);
				return Invalid;
			}
			return Valid;
		}
		
		public bool SupportActivationIsIllegal(Player player, Support support)
		{
			//if (!player.Active())
			if (!(player.State.GetType() == typeof(Idle) || player.State.GetType() == typeof(Active)))
			{
				Disqualify(player, Reasons.InvalidActivation);
				return Invalid;
			}

			else if (!support.Ready)
			{
				Disqualify(player, Reasons.InvalidActivation);
				return Invalid;
			}
			
			else if (!support.CanBeActivated)
			{
				Disqualify(player, Reasons.InvalidActivation);
				return Invalid;
			}
			
			else if (!player.Support.Contains(support))
			{
				Disqualify(player, Reasons.InvalidActivation);
				return Invalid;
			}
			
			else if (support.Activated)
			{	
				Disqualify(player, Reasons.InvalidActivation);
				return Invalid;
			}
			return Valid;
		}
		
		public bool SettingFacedownIsIllegal(Player player, Support support)
		{
			if (player.State.GetType() != typeof(Idle))
			{
				Disqualify(player, Reasons.InvalidSet);
				return Invalid;
			}
			
			else if (!player.Hand.Contains(support))
			{
				Disqualify(player, Reasons.InvalidSet);
				return Invalid;
			}
			
			else if (!support.CanBeSet)
			{
				Disqualify(player, Reasons.InvalidSet);
				return Invalid;
			}
			return Valid;
		}
		
		public bool EndingTurnIsIllegal(Player player)
		{
			if (player.State.GetType() != typeof(Idle))
			{
				Disqualify(player, Reasons.InvalidEndTurn);
				return Invalid;
			}
			return Valid;
		}
	}
}
