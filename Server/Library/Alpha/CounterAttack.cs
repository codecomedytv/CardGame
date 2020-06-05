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

        public class DestroyAttacker: Skill
        {
            public DestroyAttacker()
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