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
            Skill = new DrawCards(this);
        }

        private class DrawCards : Manual
        {
            public DrawCards(Card card)
            {
                Card = card;
                AreaOfEffects.Add(Controller.Support);

            }
            protected override void _Resolve()
            {
                Draw(Controller, 2);
            }
        }
    }
}