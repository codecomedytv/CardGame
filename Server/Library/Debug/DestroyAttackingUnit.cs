using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;
using Godot;

namespace CardGame.Server
{
    public class DestroyAttackingUnit : Support
    {
        public DestroyAttackingUnit()
        {
            Title = "Debug.DestroyAttackingUnit";
            SetCode = SetCodes.DebugDestroyAttackingUnit;
            AddSkill(new DestroyAttacking());
        }

        private class DestroyAttacking : Skill
        {
            public DestroyAttacking()
            {
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