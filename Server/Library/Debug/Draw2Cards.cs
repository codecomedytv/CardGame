using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skills;

namespace CardGame.Server
{
    public class Draw2Cards : Support
    {
        public Draw2Cards(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.Draw2Cards";
            SetCode = SetCodes.DebugDraw2Cards;
            AddSkill(new DrawCards());
        }

        private class DrawCards : Skill
        {
            protected override void _Resolve()
            {
                Controller.Draw();
                Controller.Draw();
            }
        }
    }
}