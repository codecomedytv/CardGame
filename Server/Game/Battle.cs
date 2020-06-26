using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;
using CardGame.Server.Game.Tags;
using Godot;

namespace CardGame.Server.Game {

	public class Battle : Reference, IResolvable
	{
		private const int DirectAttack = -1;
		private Player Attacking;
		private Player Defending;
		private Unit Attacker;
		private Unit Defender;
		private bool IsDirectAttack;
		
		public void Begin(Player attacking, Unit attacker, Unit defender)
		{
			Attacking = attacking;
			Defending = attacking.Opponent;
			Attacker = attacker;
			Defender = defender;
			IsDirectAttack = false;
		}
		
		public void BeginDirectAttack(Player player, Unit attacker)
		{
			Attacking = player;
			Defending = player.Opponent;
			Attacker = attacker;
			IsDirectAttack = true;
		}

		private void _ResolveAttackUnit()
		{
			Attacker.Attacked = true;
			if (!Attacking.Field.Contains(Attacker) || !Defending.Field.Contains(Defender))
			{
				return;
			}
			
			if (Attacker.Attack > Defender.Defense)
			{
				var overflow = Attacker.Attack - Defender.Defense;
				if(!Defending.HasTag(TagIds.CannotTakeBattleDamage))
				{
					Defending.Match.History.Add(new ModifyPlayer(GameEvents.BattleDamage, Attacker, Defending,
						nameof(Player.Health), Defending.Health - overflow));
				}
				Defending.Match.History.Add(new Move(GameEvents.DestroyByBattle, Attacker, Defender, Defender.Owner.Graveyard));
				if (Defending.Health <= 0)
				{
					Attacking.Win();
				}
				
				Attacker.Exhaust();
			}
			
			else if (Attacker.Attack <= Defender.Defense && Defender.Attack > Attacker.Defense)
			{
				var overflow = Defender.Attack - Attacker.Defense;
				if (!Attacking.HasTag(TagIds.CannotTakeBattleDamage))
				{
					Attacking.Match.History.Add(new ModifyPlayer(GameEvents.BattleDamage, Defender, Attacking,
						nameof(Player.Health), Attacking.Health - overflow));
				}

				Attacking.Match.History.Add(new Move(GameEvents.DestroyByBattle, Defender, Attacker, Attacker.Owner.Graveyard));
				if (Attacking.Health <= 0)
				{
					Defending.Win();
				}
			
				Attacker.Exhaust();
			}

			else
			{
				Attacker.Exhaust();
			}
		}

		private void _ResolveDirectAttack()
		{
			Attacker.Attacked = true;
			if (!Defending.HasTag(TagIds.CannotTakeBattleDamage))
			{
				Defending.Match.History.Add(new ModifyPlayer(GameEvents.BattleDamage, Attacker, Defending,
					nameof(Player.Health), Defending.Health - Attacker.Attack));
			}

			if (Defending.Health <= 0)
			{
				Attacking.Win();
			}
			
			Attacker.Exhaust();
		}
		
		public void Resolve()
		{
			if (IsDirectAttack) { _ResolveDirectAttack(); } else { _ResolveAttackUnit(); }
		}
	}
}
