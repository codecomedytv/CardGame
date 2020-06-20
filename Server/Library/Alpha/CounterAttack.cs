using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;

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
                Controller.DeclarePlay(new Move(Card, GameState.Attacking.Owner, GameState.Attacking, GameState.Attacking.Owner.Graveyard));
            }
        }
    }
}