using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;
using CardGame.Server.Game.Tags;
using Godot;

namespace CardGame.Server.Game.Cards
{
    public class Unit : Card
    {
        public int Attack = 0;
        public int Defense = 0;
        public bool Attacked = false;
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

        public void AttackDirectly()
        {
            var directAttack = new DirectAttack(this, History);
            History.Add(new DeclareDirectAttack(this, directAttack));
        }
        
        public class DirectAttack : Godot.Object, IResolvable
        {

            private readonly Unit Attacker;
            private readonly History History;

            public DirectAttack(Unit attacker, History history)
            {
                Attacker = attacker;
                History = history;
            }

            public void Resolve()
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
            }
        }
    }
}
