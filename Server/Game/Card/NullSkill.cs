namespace CardGame.Server
{
    public class NullSkill: Skill
    {
        public override void _SetUp()
        {
            CanBeUsed = false;
        }
    }
}