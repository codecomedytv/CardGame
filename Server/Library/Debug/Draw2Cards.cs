namespace CardGame.Server
{
    public class Draw2Cards : Support
    {
        public Draw2Cards()
        {
            Title = "Debug.Draw2Cards";
            SetCode = SetCodes.DebugDraw2Cards;
            AddSkill(new DrawCards());
        }

        public class DrawCards : Skill
        {
            public override void _Resolve()
            {
                Controller.Draw(2);
            }
        }
    }
}