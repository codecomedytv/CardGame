namespace CardGame.Server.Game.Skills
{
    public class NullSkill: Skill
    {
        protected override void _SetUp()
        {
            CanBeUsed = false;
        }
    }
}