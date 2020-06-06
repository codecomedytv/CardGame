using Godot;
using System;

namespace CardGame.Server {

	public class Battle : Reference, IResolvable
	{
		private const int DirectAttack = -1;
		private Player Attacking;
		private Player Defending;
		private Unit Attacker;
		private object Defender;
		private bool IsDirectAttack;
		
		public void Begin(Player attacking, Unit attacker, object defender, bool isDirectAttack)
		{
			Attacking = attacking;
			Defending = attacking.Opponent;
			Attacker = attacker;
			Defender = defender;
			IsDirectAttack = isDirectAttack;
		}
		
		public void Resolve(string ignore = "")
		{
			Attacker.Attacked = true;
			if (!Attacking.Field.Contains(Attacker))
			{
				return;
			}

			if (!IsDirectAttack && !Defending.Field.Contains((Card)Defender))
			{
				return;
			}

			if (IsDirectAttack)
			{
				// Defender is not a Card, so it must be int and the only int is directAttack
				Attacking.AttackDirectly(Attacker);
				Defending.LoseLife(Attacker.Attack);
				Attacking.DeclarePlay(new UnreadyCard(Attacker));
				return;
			}

			Attacking.AttackUnit(Attacker, Defender as Unit);

			var defender = (Unit)Defender;
			if (Attacker.Attack > defender.Defense)
			{
				var overflow = Attacker.Attack - defender.Defense;
				if (!defender.HasTag(Tag.CannotBeDestroyedByBattle))
				{
					Defending.DestroyUnit(Defender as Unit);
				}

				Defending.LoseLife(overflow);
				Attacking.DeclarePlay(new UnreadyCard(Attacker));
			}
			
			else if (Attacker.Attack <= defender.Defense && defender.Attack > Attacker.Defense)
			{
				var overflow = defender.Attack - Attacker.Defense;
				if (!Attacker.HasTag(Tag.CannotBeDestroyedByBattle))
				{
					Attacking.DestroyUnit(Attacker);
				}

				Attacking.LoseLife(overflow);
				Attacking.DeclarePlay(new UnreadyCard(Attacker));
			}

			else
			{
				Attacking.DeclarePlay(new UnreadyCard(Attacker));
			}
			{
				
			}
		}
	}

}
