using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using Godot;

namespace CardGame.Server.Game {

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
				if(!Defending.HasTag(Tag.CannotTakeDamage))
				{
					//Defending.DeclarePlay(new LoseLife(Attacker, Defending, Attacker.Attack));
					Defending.DeclarePlay(new ModifyPlayer(Attacker, Defending, nameof(Player.Health), Defending.Health - Attacker.Attack));
					if (Defending.Health <= 0)
					{
						Attacking.Win();
					}
				}
				Attacking.DeclarePlay(new Modify(Attacker, nameof(Card.Ready), false));
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

				if(!Defending.HasTag(Tag.CannotTakeDamage))
				{
					//Defending.DeclarePlay(new LoseLife(Attacker, Defending, overflow));
					Defending.DeclarePlay(new ModifyPlayer(Attacker, Defending, nameof(Player.Health), Defending.Health - overflow));
					if (Defending.Health <= 0)
					{
						Attacking.Win();
					}
				}
				Attacking.DeclarePlay(new Modify(Attacker, nameof(Card.Ready), false));
			}
			
			else if (Attacker.Attack <= defender.Defense && defender.Attack > Attacker.Defense)
			{
				var overflow = defender.Attack - Attacker.Defense;
				if (!Attacker.HasTag(Tag.CannotBeDestroyedByBattle))
				{
					Attacking.DestroyUnit(Attacker);
				}

				if(!Attacking.HasTag(Tag.CannotTakeDamage))
				{
					//Attacking.DeclarePlay(new LoseLife((Unit)Defender, Attacking, overflow));
					Attacking.DeclarePlay(new ModifyPlayer(defender, Attacking, nameof(Player.Health), Attacking.Health - overflow));
					if (Attacking.Health <= 0)
					{
						Defending.Win();
					}
				}
				Attacking.DeclarePlay(new Modify(Attacker, nameof(Card.Ready), false));
			}

			else
			{
				Attacking.DeclarePlay(new Modify(Attacker, nameof(Card.Ready), false));
			}
			{
				
			}
		}
	}

}
