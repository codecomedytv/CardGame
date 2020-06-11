using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;

namespace CardGame.Server
{
    public class ReturnCardToDeck: Support
    {
        public ReturnCardToDeck()
        {
            Title = "Debug.ReturnToDeck";
            SetCode = SetCodes.DebugReturnToDeck;
            AddSkill(new ReturnCard());
        }

        private class ReturnCard : Skill
        {
            public override void _SetUp()
            {
                SetTargets(Controller.Hand);
                CanBeUsed = Controller.Hand.Count > 0;
            }

            protected override void _Resolve()
            {
                Controller.DeclarePlay( new ReturnToDeck(Card, Controller, Target));
            }
        }
        
    }
}