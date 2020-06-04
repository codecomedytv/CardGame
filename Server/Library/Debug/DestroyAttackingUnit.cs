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

        public class DestroyAttacking : Skill
        {
            public DestroyAttacking()
            {
                GameEvent = "attack";
            }

            public override void _Resolve()
            {
                Controller.DestroyUnit(GameState.Attacking);
            }
        }
        
    }
}