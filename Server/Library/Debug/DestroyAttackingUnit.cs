using CardGame.Server.Game.Cards;
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
                GameEvent = "attack";
            }

            protected override void _Resolve()
            {
                Controller.DestroyUnit(GameState.Attacking);
            }
        }
        
    }
}