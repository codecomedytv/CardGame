﻿using System.Collections.Generic;
using System.Linq;

namespace CardGame.Server
{
    public class Unit: Card
    {
        public int Attack = 0;
        public int Defense = 0;
        public bool Attacked = false;
        public List<Card> ValidAttackTargets = new List<Card>();
        public bool CanBeDeployed = false;

        public Unit()
        {
            // We could possibly move these up to the base card
            SetAttributes();
            SetSkillCards();
            CreateSkills();
        }
        
        public override void OnControllerStateChanged(int state, string signal)
        {
            throw new System.NotImplementedException();
        }

        protected override void SetAttributes()
        {
            throw new System.NotImplementedException();
        }

        protected override void _SetAsPlayable()
        {
            // TODO: Add Tag Check
            CanBeDeployed = true;
            Controller.DeclarePlay(new SetAsDeployable(this));
        }

        public void SetAsAttacker()
        {
            SetValidAttackTargets();
        }

        public void SetValidAttackTargets()
        {
            if (!Ready)
            {
                return;
            }
            
            ValidAttackTargets.Clear();
            foreach (var unit in Opponent.Field.Where(u => !u.HasTag(Tag.CannotBeAttacked)))
            {
                ValidAttackTargets.Add(unit);
            }
            
            Controller.DeclarePlay(new SetTargets(this, ValidAttackTargets.ToList()));
        }

    }
    
}