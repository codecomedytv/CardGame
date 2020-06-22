using CardGame.Server.Room.Cards;

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

        private class DrawCards : Skill
        {
            protected override void _Resolve()
            {
                Controller.DrawCards(2);
            }
        }
    }
}