using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skill;

namespace CardGame.Server
{
    public class CounterAttack: Support
    {
        public CounterAttack()
        {
            Title = "CounterAttack";
            SetCode = SetCodes.Alpha_CounterAttack;
            AddSkill(new DestroyAttacker());
        }

        private class DestroyAttacker: Skill
        {
            public DestroyAttacker()
            {
                GameEvent = "attack";
            }

            protected override void _Resolve()
            {
                Controller.Match.History.Add(new Move(GameEvents.DestroyByEffect, Card, Match.Attacking, Match.Attacking.Owner.Graveyard));
            }
        }
    }
}