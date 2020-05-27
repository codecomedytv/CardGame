namespace CardGame.Server
{
    public class PlayerCannotTakeDamage : Unit
    {
        public PlayerCannotTakeDamage()
        {
            Title = "Debug.PlayerCannotTakeDamage";
            Attack = 1000;
            Defense = 1000;
            SetCode = SetCodes.DebugPlayerCannotTakeDamage;
            AddSkill(new CannotTakeDamage());
        }

        public class CannotTakeDamage : Skill
        {
            public CannotTakeDamage()
            {
                GameEvent = "deploy";
                Type = Types.Constant;
            }

            public override void _Resolve()
            {
                AddTagToController(Tag.CannotTakeDamage);
            }
        }
    }
}