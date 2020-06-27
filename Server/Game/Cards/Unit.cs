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
        

    }
    
}
