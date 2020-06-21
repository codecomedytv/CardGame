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
		
		public void BeginDirectAttack(Player player, Unit attacker)
		{
			Attacking = player;
			Defending = player.Opponent;
			Attacker = attacker;
			IsDirectAttack = true;
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
				Attacking.AttackDirectly(Attacker);
				Defending.DeclarePlay(new ModifyPlayer(Attacker, Defending, nameof(Player.Health), Defending.Health - Attacker.Attack));
				if (Defending.Health <= 0)
				{
					Attacking.Win();
				}
			
				Attacking.DeclarePlay(new Modify(Attacking, Attacker, nameof(Card.Ready), false));
				return;
			}

			Attacking.AttackUnit(Attacker, Defender as Unit);

			var defender = (Unit)Defender;
			if (Attacker.Attack > defender.Defense)
			{
				var overflow = Attacker.Attack - defender.Defense;
				Defending.DeclarePlay(new ModifyPlayer(Attacker, Defending, nameof(Player.Health), Defending.Health - overflow));
				Defending.DeclarePlay(new Move(Attacker, Defending, Defender as Unit, ((Unit) Defender).Owner.Graveyard));
				if (Defending.Health <= 0)
				{
					Attacking.Win();
				}
				
				Attacking.DeclarePlay(new Modify(Attacking, Attacker, nameof(Card.Ready), false));
			}
			
			else if (Attacker.Attack <= defender.Defense && defender.Attack > Attacker.Defense)
			{
				var overflow = defender.Attack - Attacker.Defense;
				Attacking.DeclarePlay(new ModifyPlayer(defender, Attacking, nameof(Player.Health), Attacking.Health - overflow));
				Attacking.DeclarePlay(new Move(Defender as Card, Attacking, Attacker, Attacker.Owner.Graveyard));
				if (Attacking.Health <= 0)
				{
					Defending.Win();
				}
			
				Attacking.DeclarePlay(new Modify(Attacking, Attacker, nameof(Card.Ready), false));
			}

			else
			{
				Attacking.DeclarePlay(new Modify(Attacking, Attacker, nameof(Card.Ready), false));
			}
			{
				
			}
		}


	}

}
