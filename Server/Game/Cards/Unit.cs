﻿using System;
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

        public void AttackTarget(Unit defender)
        {
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
                    Attacker.Opponent.Health -= Attacker.Attack;
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

            public AttackUnit(Unit attacker, Unit defender, History history)
            {
                Attacker = attacker;
                Defender = defender;
                History = history;
            }

            public void Resolve()
            {
                var attacking = Attacker.Controller;
                var defending = Defender.Controller;
                if (!attacking.Field.Contains(Attacker) || !defending.Field.Contains(Defender))
                {
                    return;
                }

                if (Attacker.Attack > Defender.Defense)
                {
                    if (!defending.HasTag(TagIds.CannotTakeBattleDamage))
                    {
                        var overflow = Attacker.Attack - Defender.Defense;
                        var oldLife = defending.Health;
                        defending.Health -= overflow;
                        var newLife = defending.Health;
                        History.Add(new ModifyPlayer(GameEvents.BattleDamage, Attacker, defending,
                            nameof(Player.Health), oldLife, newLife));
                    }

                    History.Add(new Move(GameEvents.DestroyByBattle, Attacker, Defender, Defender.Owner.Graveyard));
                }
                else if (Attacker.Attack <= Defender.Defense && Defender.Attack > Attacker.Defense)
                {
                    var overflow = Defender.Attack - Attacker.Defense;
                    if (!attacking.HasTag(TagIds.CannotTakeBattleDamage))
                    {
                        var oldLife = attacking.Health;
                        attacking.Health -= overflow;
                        var newLife = attacking.Health;
                        History.Add(new ModifyPlayer(GameEvents.BattleDamage, Defender, attacking,
                            nameof(Player.Health), oldLife, newLife));
                    }

                    History.Add(new Move(GameEvents.DestroyByBattle, Defender, Attacker, Attacker.Owner.Graveyard));
                    if (attacking.Health <= 0)
                    {
                        defending.Win();
                    }

                    Attacker.Exhaust();
                }
            }
        }
    }
}