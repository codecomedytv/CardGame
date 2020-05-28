using Godot;
using System;
using System.Linq;

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
		
		public bool DeployIsIllegalPlay(Gamestate state, Player player, Unit unit)
		{
			if(player.State != Player.States.Idle)
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
		
		public bool AttackDeclarationIsIllegal(Gamestate state, Player player, Unit attacker, int defenderId)
		{
			if (player.State != Player.States.Idle)
			{
				Disqualify(player, Reasons.InvalidAttack);
				return Invalid;
			}
			
			if (!attacker.Ready)
			{
				Disqualify(player, Reasons.InvalidAttack);
				return Invalid;
			}

			if (defenderId == -1 && player.Opponent.Field.Count != 0)
			{
				Disqualify(player, Reasons.InvalidAttack);
				return Invalid;
			}
			
			if (defenderId > 0 && !player.Opponent.Field.Contains(state.GetCard(defenderId)))
			{
				Disqualify(player, Reasons.InvalidAttack);
				return Invalid;
			}
			if (defenderId > 0 && !attacker.ValidAttackTargets.Contains(state.GetCard(defenderId)))
			{
				Disqualify(player, Reasons.InvalidAttack);
				return Invalid;
			}
			return Valid;
		}
		
		public bool SupportActivationIsIllegal(Gamestate state, Player player, Support support)
		{
			if (!player.Active())
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
		
		public bool SettingFacedownIsIllegal(Gamestate state, Player player, Support support)
		{
			if (player.State != Player.States.Idle)
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
		
		public bool EndingTurnIsIllegal(Gamestate state, Player player)
		{
			if (player.State != Player.States.Idle)
			{
				Disqualify(player, Reasons.InvalidEndTurn);
				return Invalid;
			}
			return Valid;
		}
	}
}
