using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;
using Godot;

namespace CardGame.Server
{
    public class DestroyAttackingUnit : Support
    {
        public DestroyAttackingUnit(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.DestroyAttackingUnit";
            SetCode = SetCodes.DebugDestroyAttackingUnit;
            Skill = new DestroyAttacking(this);
        }

        private class DestroyAttacking : Skill
        {
            public DestroyAttacking(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Support);
                Triggers.Add(GameEvents.DeclareAttack);
                Triggers.Add(GameEvents.DeclareDirectAttack);
            }

            protected override void _Resolve()
            {
                Destroy(Match.Attacking);
            }
        }
        
    }
}