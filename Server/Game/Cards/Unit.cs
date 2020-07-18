using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Events;
using CardGame.Server.Game.Skills;
using CardGame.Server.Game.Tags;
using CardGame.Server.Game.Zones;
using Godot;

namespace CardGame.Server.Game.Cards
{
    public class Unit : Card
    {
        public int Power = 0;
        public bool Attacked = false;
        public IEnumerable<Card> ValidAttackTargets = new List<Card>();

        protected Unit()
        {
        }

        public override List<int> GetValidAttackTargets() => ValidAttackTargets.Select(c => c.Id).ToList();

        public override void SetState()
        {
            State = States.Passive;
            if (Zone == Controller.Hand && Controller.Field.Count < 7)
            {
                State = States.CanBeDeployed;
            }

            if (Zone == Controller.Field && IsReady && !Attacked)
            {
                ValidAttackTargets = Opponent.Field.AsEnumerable();
                State = States.CanAttack;
            }
        }
        
        public void AttackDirectly()
        {
            Controller.AttackingWith = this;
            var directAttack = new DirectAttack(this, History);
            History.Add(new DeclareDirectAttack(this, directAttack));
        }

        public void AttackTarget(Unit defender)
        {
            Controller.AttackingWith = this;
            var attackUnit = new AttackUnit(this, defender, History);
            History.Add(new DeclareAttack(this, defender, attackUnit));
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
                    var oldLife = Attacker.Opponent.Health;
                    Attacker.Opponent.Health -= Attacker.Power;
                    var newLife = Attacker.Opponent.Health;
                    History.Add(new ModifyPlayer(GameEvents.BattleDamage, Attacker, Attacker.Opponent,
                        nameof(Player.Health), oldLife, newLife));
                }

                if (Attacker.Opponent.Health <= 0)
                {
                    Attacker.Controller.Win();
                }

                Attacker.Exhaust();
            }
        }

        public class AttackUnit : IResolvable
        {
            private readonly Unit Attacker;
            private readonly Unit Defender;
            private readonly History History;
            private readonly Player defending;
            private readonly Player attacking;

            public AttackUnit(Unit attacker, Unit defender, History history)
            {
                Attacker = attacker;
                Defender = defender;
                History = history;
                attacking = attacker.Controller;
                defending = defender.Controller;
            }

            private void Attack()
            {
                if (!defending.HasTag(TagIds.CannotTakeBattleDamage))
                {
                    var overflow = Attacker.Power - Defender.Power;
                    defending.Health -= overflow;
                    History.Add(new BattleDamage(Attacker, defending, overflow));
                }
                
                // MoveSkillHere?
                defending.Field.Remove(Defender);
                defending.Graveyard.Add(Defender);
                Defender.Zone = Defender.Owner.Graveyard;
                
                History.Add(new DestroyByBattle(Attacker, defending, Defender));
                History.Add(new SentToZone(defending, Defender, ZoneIds.Graveyard));
                if (defending.Health <= 0)
                {
                    attacking.Win();
                }
            }

            private void CounterAttack()
            {
                if (!attacking.HasTag(TagIds.CannotTakeBattleDamage))
                {
                    var overflow = Defender.Power - Attacker.Power;
                    attacking.Health -= overflow;
                    History.Add(new BattleDamage(Defender, attacking, overflow));
                }

                // MoveSkillHere?
                attacking.Field.Remove(Attacker);
                attacking.Graveyard.Add(Attacker);
                Attacker.Zone = Attacker.Owner.Graveyard;
                
                History.Add(new DestroyByBattle(Defender, attacking, Attacker));
                History.Add(new SentToZone(attacking, Attacker, ZoneIds.Graveyard));
                
                if (attacking.Health <= 0)
                {
                    defending.Win();
                }
            }


            public void Resolve()
            {
                if (!attacking.Field.Contains(Attacker) || !defending.Field.Contains(Defender))
                {
                    return;
                }
                
                History.Add(new BattleUnit(attacking, Attacker, Defender));
                if (Attacker.Power > Defender.Power)
                {
                    Attack();
                }
                else if (Attacker.Power < Defender.Power)
                {
                    CounterAttack();
                }
                else
                {
                    GD.Print("Tie?");
                }

                Attacker.Exhaust();
            }
        }
    }
}