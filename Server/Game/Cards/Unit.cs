using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Commands;

namespace CardGame.Server.Game.Cards
{
    public class Unit: Card
    {
        public int Attack = 0;
        public int Defense = 0;
        public bool Attacked = false;
        public List<Card> ValidAttackTargets = new List<Card>();

        public Unit()
        {
        }

        public override void SetCanBeDeployed()
        {
            if (Zone == Controller.Hand && Controller.Field.Count < 7) 
            { Controller.Match.History.Add(new ModifyCard(Controller, this, nameof(CanBeDeployed), this)); }
        }

        public override void SetCanAttack()
        {
            if (Zone != Controller.Field || !Ready || Attacked) return;
            ValidAttackTargets = Opponent.Field.ToList();
            Controller.Match.History.Add(new ModifyCard(Controller, this, nameof(CanAttack), true));
            // TODO: Re-implement Setting Attack Targets
        }
        

    }
    
}
