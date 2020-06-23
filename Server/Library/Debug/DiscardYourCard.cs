using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skill;

namespace CardGame.Server
{
    public class DiscardYourCard : Support
    {
        public DiscardYourCard()
        {
            Title = "Debug.DiscardYourCard";
            SetCode = SetCodes.DebugDiscardYourCard;
            AddSkill(new DiscardCard());
        }

        private class DiscardCard : Skill
        {
            public override void _SetUp()
            {
                SetTargets(Controller.Hand.ToList());
                CanBeUsed = Controller.Hand.Count > 0;
            }

            protected override void _Resolve()
            {
                Controller.DeclarePlay(new Move(Card, Target, Target.Owner.Graveyard));
            }
        }
    }
}