using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace CardGame.Server
{
    public class Unit: Card
    {
        public int Attack = 0;
        public int Defense = 0;
        public bool Attacked = false;
        public List<Card> ValidAttackTargets = new List<Card>();

        public Unit()
        {
            Tags = new List<Decorator>();
        }

        public override void SetCanBeDeployed()
        {
            CanBeDeployed = Zone == Controller.Hand && Controller.Field.Count < 7;
            if (CanBeDeployed) { Controller.DeclarePlay(new SetAsDeployable(this)); }
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
