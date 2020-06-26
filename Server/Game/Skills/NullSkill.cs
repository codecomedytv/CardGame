namespace CardGame.Server.Game.Skills
{
    public class NullSkill: Manual
    {
        protected override void _SetUp()
        {
            CanBeUsed = false;
        }
    }
}