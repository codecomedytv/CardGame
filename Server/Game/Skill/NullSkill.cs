namespace CardGame.Server.Game.Skill
{
    public class NullSkill: Skill
    {
        public override void _SetUp()
        {
            CanBeUsed = false;
        }
    }
}