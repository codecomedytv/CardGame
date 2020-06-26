namespace CardGame.Server.Game.Skills
{
    public class NullSkill: Skill
    {
        public override void _SetUp()
        {
            CanBeUsed = false;
        }
    }
}