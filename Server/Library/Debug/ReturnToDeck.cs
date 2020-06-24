using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skill;

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
                SetTargets(Controller.Hand.ToList());
                CanBeUsed = Controller.Hand.Count > 0;
            }

            protected override void _Resolve()
            {
                Controller.DeclarePlay(new Move(GameEvents.TopDeck, Card, Target, Target.Owner.Deck));
            }
        }
        
    }
}