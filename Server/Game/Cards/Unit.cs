using System.Collections.Generic;
using System.Linq;

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
            Tags = new List<Decorator>();
        }

        public override void SetCanBeDeployed()
        {
            CanBeDeployed = Zone == Controller.Hand && Controller.Field.Count < 7;
            if (CanBeDeployed) { Controller.DeclarePlay(new SetAsDeployable(this)); }
        }

        public override void SetCanAttack()
        {
            CanAttack = Zone == Controller.Field && Ready && !Attacked;
            if (CanAttack)
            {
                ValidAttackTargets = Opponent.Field.Where(u => !u.HasTag(Tag.CannotBeAttacked)).ToList();
                Controller.DeclarePlay(new SetAsAttacker(this));
                Controller.DeclarePlay(new SetTargets(this, ValidAttackTargets));
            }
        }
        

    }
    
}
