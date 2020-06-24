using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skill;
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

			Attacking.DeclarePlay(new DeclareAttack(Attacker, Defender));
			
			if (Attacker.Attack > Defender.Defense)
			{
				var overflow = Attacker.Attack - Defender.Defense;
				Defending.DeclarePlay(new ModifyPlayer(Attacker, Defending, nameof(Player.Health), Defending.Health - overflow));
				Defending.DeclarePlay(new Move(GameEvents.DestroyByBattle, Attacker, Defender, Defender.Owner.Graveyard));
				if (Defending.Health <= 0)
				{
					Attacking.Win();
				}
				
				Attacking.DeclarePlay(new ModifyCard(Attacking, Attacker, nameof(Card.Ready), false));
			}
			
			else if (Attacker.Attack <= Defender.Defense && Defender.Attack > Attacker.Defense)
			{
				var overflow = Defender.Attack - Attacker.Defense;
				Attacking.DeclarePlay(new ModifyPlayer(Defender, Attacking, nameof(Player.Health), Attacking.Health - overflow));
				Attacking.DeclarePlay(new Move(GameEvents.DestroyByBattle, Defender, Attacker, Attacker.Owner.Graveyard));
				if (Attacking.Health <= 0)
				{
					Defending.Win();
				}
			
				Attacking.DeclarePlay(new ModifyCard(Attacking, Attacker, nameof(Card.Ready), false));
			}

			else
			{
				Attacking.DeclarePlay(new ModifyCard(Attacking, Attacker, nameof(Card.Ready), false));
			}
		}

		private void _ResolveDirectAttack()
		{
			Attacker.Attacked = true;
			Attacking.DeclarePlay(new DeclareDirectAttack(Attacker));
			Defending.DeclarePlay(new ModifyPlayer(Attacker, Defending, nameof(Player.Health), Defending.Health - Attacker.Attack));
			if (Defending.Health <= 0)
			{
				Attacking.Win();
			}
			
			Attacking.DeclarePlay(new ModifyCard(Attacking, Attacker, nameof(Card.Ready), false));
		}
		
		public void Resolve(string ignore = "")
		{
			if (IsDirectAttack) { _ResolveDirectAttack(); } else { _ResolveAttackUnit(); }
		}
	}
}
