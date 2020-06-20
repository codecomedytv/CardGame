using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
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
                Controller.DeclarePlay(new Move(Card, GameState.Attacking.Owner, GameState.Attacking, GameState.Attacking.Owner.Graveyard));
            }
        }
        
    }
}