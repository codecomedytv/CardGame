using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skill;
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
                Controller.DeclarePlay(new Move(GameEvents.DestroyByEffect, Card, Match.Attacking, Match.Attacking.Owner.Graveyard));
            }
        }
        
    }
}