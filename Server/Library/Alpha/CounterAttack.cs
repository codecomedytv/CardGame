using CardGame.Server.Room.Cards;
using CardGame.Server.Room.Commands;

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
                Controller.DeclarePlay(new Move(Card, Game.Attacking, Game.Attacking.Owner.Graveyard));
            }
        }
    }
}