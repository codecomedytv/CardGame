#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;
using CardGame.Server.Game.Tags;

namespace CardGame.Server.Game.Cards
{
    public class Unit: Card
    {
        public int Attack = 0;
        public int Defense = 0;
        public bool Attacked = false;
        public Battle? DeclaredAttack;
        public IEnumerable<Card> ValidAttackTargets = new List<Card>();

        protected Unit()
        {
        }

        public override void SetCanBeDeployed()
        {
            CanBeDeployed = Zone == Controller.Hand && Controller.Field.Count < 7;
        }

        public override void SetCanAttack()
        {
            if (Zone != Controller.Field || !IsReady || Attacked) return;
            ValidAttackTargets = Opponent.Field.AsEnumerable();
            CanAttack = true;
        }

        public void DeclareDirectAttack()
        {
            DeclaredAttack = new DirectAttack(this, History);
            History.Add(new DeclareDirectAttack(this));
        }

    public class AttackUnit: Battle
    {
        public void Declare()
        {
            
        }

        public override void Resolve()
        {
            
        }
    }

    public class DirectAttack: Battle
    {
        private readonly Unit Attacker;
        private readonly History History;

        public DirectAttack(Unit attacker, History history)
        {
            Attacker = attacker;
            History = history;
            History.Add(new DeclareDirectAttack(attacker));
        }

        public override void Resolve()
        {
            Attacker.Attacked = true;
            if (!Attacker.Opponent.HasTag(TagIds.CannotTakeBattleDamage))
            {
                History.Add(new ModifyPlayer(GameEvents.BattleDamage, Attacker, Attacker.Opponent,
                    nameof(Player.Health), Attacker.Opponent.Health - Attacker.Attack));
            }

            if (Attacker.Opponent.Health <= 0)
            {
                Attacker.Controller.Win();
            }
            
            Attacker.Exhaust();
            Attacker.DeclaredAttack = null;
        }
    }


    public abstract class Battle : IResolvable
    {
        public virtual void Resolve()
        {
            throw new NotImplementedException();
        }
    }

    }
    
}

/*
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
*/
